#include <iostream>
#include <gl/glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
// ID шейдерной программы
GLuint Program;
// ID атрибута
GLint Attrib_vertex;
// ID Vertex Buffer Object
GLuint VBO;

struct Vertex {
	GLfloat x;
	GLfloat y;
};

// Исходный код вершинного шейдера
const char* VertexShaderSource = R"(
#version 330 core
in vec2 coord;
void main() {
gl_Position = vec4(coord, 0.0, 1.0);
}
)";

// Исходный код фрагментного шейдера
const char* FragShaderSource = R"(
	#version 330 core
	out vec4 color;
	uniform vec4 objectColor;                 //// вот та самая переменная
	void main() {
		color = objectColor;
	}
)";

void ShaderLog(unsigned int shader)
{
	int infologLen = 0;
	glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
	if (infologLen > 1)
	{
		int charsWritten = 0;
		std::vector<char> infoLog(infologLen);
		glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog.data());
		std::cout << "InfoLog: " << infoLog.data() << std::endl;
	}
}

void checkOpenGLerror()
{
	GLenum err;
	while ((err = glGetError()) != GL_NO_ERROR)
	{
		// Process/log the error.
		std::cout << err << std::endl;
	}
}

void InitShader() {
	// Создаем вершинный шейдер
	GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
	// Передаем исходный код
	glShaderSource(vShader, 1, &VertexShaderSource, NULL);
	// Компилируем шейдер
	glCompileShader(vShader);
	std::cout << "vertex shader \n";
	// Функция печати лога шейдера
	ShaderLog(vShader);
	// Создаем фрагментный шейдер
	GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
	// Передаем исходный код
	glShaderSource(fShader, 1, &FragShaderSource, NULL);
	// Компилируем шейдер
	glCompileShader(fShader);
	std::cout << "fragment shader \n";
	// Функция печати лога шейдера
	ShaderLog(fShader);
	// Создаем программу и прикрепляем шейдеры к ней
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);
	// Линкуем шейдерную программу
	glLinkProgram(Program);
	// Проверяем статус сборки
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
	if (!link_ok) {
		std::cout << "error attach shaders \n";
		return;
	}
	// Вытягиваем ID атрибута из собранной программы
	const char* attr_name = "coord"; //имя в шейдере
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1) {
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}
	GLint objectColorLoc = glGetUniformLocation(Program, "objectColor");
	if (objectColorLoc == -1) {
		std::cout << "Could not find uniform objectColor\n";
		return;
	}
	checkOpenGLerror();
}
void InitVBO() {
	glGenBuffers(1, &VBO);
	// Вершины нашего треугольника
	Vertex square[4] = {
	{ -0.5f, -0.5f },
	{ 0.5f, -0.5f },
	{ 0.5f, 0.5f },
	{ -0.5f, 0.5f }
	};
	const int Nv = 6;
	Vertex fan[Nv] = {
		{ 0.0f, 0.0f },  // Центр веера
		{ 0.5f, 0.0f },  // Первая вершина
		{ 0.4f, 0.3f },
		{ 0.2f, 0.4f },
		{ 0.0f, 0.5f },
		{ -0.2f, 0.4f },
	};

	const int Np = 5;
	Vertex pentagon[Np];

	float radius = 0.5f; // Радиус пятиугольника
	float angle = 360.0f / Np; // Угол между вершинами

	// Вычисление координат вершин
	for (int i = 0; i < Np; ++i) {
		float theta = angle * i * 3.14159f / 180.0f; // Перевод угла в радианы
		pentagon[i] = {
			radius * cos(theta), // X координата
			radius * sin(theta)  // Y координата
		};
	}
	// Передаем вершины в буфер
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	//glBufferData(GL_ARRAY_BUFFER, sizeof(square), square, GL_STATIC_DRAW);
	glBufferData(GL_ARRAY_BUFFER, sizeof(fan), fan, GL_STATIC_DRAW);
	//glBufferData(GL_ARRAY_BUFFER, sizeof(pentagon), pentagon, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, NULL);
	checkOpenGLerror(); //Пример функции есть в лабораторной
	 // Проверка ошибок OpenGL, если есть, то вывод в консоль тип ошибки
}
void Init() {
	// Шейдеры
	InitShader();
	// Вершинный буфер
	InitVBO();
}

void Draw() {
	float color[] = { 0.0f, 1.0f, 1.0f, 1.0f }; // Фиолетовый цвет RGBA
	glUseProgram(Program); // Устанавливаем шейдерную программу текущей
	GLint objectColorLoc = glGetUniformLocation(Program, "objectColor");
	glUniform4fv(objectColorLoc, 1, color);
	glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // Подключаем VBO
	// Указывая pointer 0 при подключенном буфере, мы указываем что данные в VBO
	glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0); // Отключаем VBO
	//glDrawArrays(GL_QUADS, 0, 4);
	//glDrawArrays(GL_TRIANGLE_FAN, 0, 5);
	glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
	glDisableVertexAttribArray(Attrib_vertex); // Отключаем массив атрибутов
	glUseProgram(0); // Отключаем шейдерную программу
	checkOpenGLerror();
}

// Освобождение буфера
void ReleaseVBO() {
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &VBO);
}

// Освобождение шейдеров
void ReleaseShader() {
	// Передавая ноль, мы отключаем шейдерную программу
	glUseProgram(0);
	// Удаляем шейдерную программу
	glDeleteProgram(Program);
}

void Release() {
	// Шейдеры
	ReleaseShader();
	// Вершинный буфер
	ReleaseVBO();
}


int main() {
	sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
	window.setVerticalSyncEnabled(true);
	window.setActive(true);
	glewInit();
	Init();
	while (window.isOpen()) {
		sf::Event event;
		while (window.pollEvent(event)) {
			if (event.type == sf::Event::Closed) { window.close(); }
			else if (event.type == sf::Event::Resized) { glViewport(0, 0, event.size.width, event.size.height); }
		}
		if (window.isOpen()) {
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			Draw();
			window.display();
		}
	}
	Release();
	return 0;

}