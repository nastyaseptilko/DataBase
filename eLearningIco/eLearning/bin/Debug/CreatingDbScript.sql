USE SAA_MyBase

CREATE TABLE Users
(
	[IdUser] int PRIMARY KEY IDENTITY,
	[Login] nvarchar(30) NOT NULL,
	[Password] nvarchar(15) NOT NULL
)

CREATE TABLE Themes
(
	[IdTheme] int PRIMARY KEY IDENTITY,
	[Name] nvarchar(100) 
)


INSERT INTO [Themes] ([Name])
VALUES ('Артикли'), ('Времена'), ('Прилагательные'), ('Существительные');

CREATE TABLE Tests
(
	[IdTest] int PRIMARY KEY IDENTITY,
	[Name] nvarchar(50) UNIQUE,
	[IdTheme] int
    CONSTRAINT FK_TESTS_TO_THEMES FOREIGN KEY ([IdTheme]) REFERENCES [Themes]([IdTheme])
)

CREATE TABLE Questions
(
	[IdQuestion] int PRIMARY KEY IDENTITY,
	[IdTest] int,
	[NumberQuestion] int,
	[Question] nvarchar(100) UNIQUE
	CONSTRAINT FK_QUESTIONS_TO_TESTS FOREIGN KEY ([IdTest]) REFERENCES [Tests]([IdTest])
)

CREATE TABLE Answer
(
	[IdAnswer] int PRIMARY KEY IDENTITY,
	[Answer] nvarchar(50),
	[IsRight] bit,
	[IdQuestion] int,
	CONSTRAINT FK_ANSWER_TO_QUESTIONS FOREIGN KEY ([IdQuestion]) REFERENCES [Questions]([IdQuestion]),
)
 
CREATE TABLE Progress
(
	[IdUser] int,
	[IdTest] int,
	[NameTest] nvarchar(50),
	[DateTest] nvarchar(50),
	[IsRight] bit,
	[CountRightAnswer] int
	CONSTRAINT FK_PROGRESS_TO_USERS FOREIGN KEY ([IdUser]) REFERENCES [Users]([IdUser]),
	CONSTRAINT FK_PROGRESS_TO_TESTS FOREIGN KEY ([IdTest]) REFERENCES [Tests]([IdTest])
) 

CREATE TABLE ThemesForDictionary
(
	[IdTheme] int PRIMARY KEY,
	[NameThemeForDictionary] nvarchar(50)
)

CREATE TABLE PodThemes
(
	[IdTheme] int PRIMARY KEY,
	[IdThemeForDictionary] int,
	[NameTheme] nvarchar(50)
	CONSTRAINT FK_PODTHEMES_TO_THEMESFORDICTIONARY FOREIGN KEY ([IdThemeForDictionary]) REFERENCES [ThemesForDictionary]([IdTheme])
)

CREATE TABLE Words
(
	[IdWord] int PRIMARY KEY,
	[IdTheme] int,
	[EnglishWord] nvarchar(30),
	[RussianWord] nvarchar(30)
	CONSTRAINT FK_WORDS_TO_PODTHEMES FOREIGN KEY ([IdTheme]) REFERENCES [PodThemes]([IdTheme])
)

CREATE TABLE ProgressDictionary
(
	[IdUser] int,
	[NameTest] nvarchar(50),
	[DateTest] nvarchar(50),
	[IsRight] bit,
	[CountRightAnswer] int,
	[CountQuestion] int
	CONSTRAINT FK_TESTDICTIONATY_TO_USERS FOREIGN KEY ([IdUser]) REFERENCES [Users]([IdUser]),
)


--------------ДЛЯ СЛОВАРЯ-----------------------------------------------------
--------------ДЛЯ СЛОВАРЯ--------------------------------------------------------

INSERT INTO [ThemesForDictionary] ([IdTheme], [NameThemeForDictionary])
VALUES (1, 'Знакомство. Общение'),
	   (2, 'Дом'),
	   (3, 'Город'),
	   (4, 'Еда');

INSERT INTO [PodThemes] ([IdTheme], [IdThemeForDictionary], [NameTheme])
VALUES (1, 1, 'Приветсвия и прощания'),
	   (2, 1, 'Формулы вежливости'),
	   (3, 2, 'Дом, квартира'),
	   (4, 2, 'Мебель, интерьер'),
	   (5, 3, 'Город'),
	   (6, 3, 'Транспорт'),
	   (7, 4, 'Овощи'),
	   (8, 4, 'Фрукты');

INSERT INTO [Words] ([IdWord], [IdTheme], [EnglishWord], [RussianWord])
VALUES 
-----DICTIONARY-1-1---------------------------------
	   (1, 1, 'Hello', 'Здравствуйте'),
	   (2, 1, 'Hi', 'Привет'),
       (3, 1, 'Goodbye', 'До свидания'),
	   (4, 1, 'Bye', 'Пока'),
	   (5, 1, 'Good morning', 'Доброе утро'),
	   (6, 1, 'See you later', 'До встречи'),
	   (7, 1, 'Good night', 'Спокойной ночи'),
-----DICTIONARY-1-2---------------------------------
       (8, 2, 'Thank you', 'Спасибо'),
	   (9, 2, 'Thanks a lot', 'Большое спасибо'),
	   (10, 2, 'You are welcome', 'Пожалуйста'),
	   (11, 2, 'Not at all', 'Не за что'),
	   (12, 2, 'No problem', 'Нет проблем'),
	   (13, 2, 'Excuse me', 'Простите'),
	   (14, 2, 'Thats OK', 'Всё нормально'),
-----DICTIONARY-2-1---------------------------------
       (15, 3, 'house', 'дом'),
	   (16, 3, 'flat', 'квартира'),
	   (17, 3, 'door', 'дверь'),
	   (18, 3, 'floor', 'пол'),
	   (19, 3, 'wall', 'стена'),
	   (20, 3, 'window', 'окно'),
	   (21, 3, 'bedroom', 'спальня'),
-----DICTIONARY-2-2---------------------------------
       (22, 4, 'bed', 'кровать'),
	   (23, 4, 'chair', 'стул'),
	   (24, 4, 'table', 'стол'),
	   (25, 4, 'lamp', 'лампа'),
	   (26, 4, 'picture', 'картина'),
	   (27, 4, 'carpet', 'ковёр'),
	   (28, 4, 'mirror', 'зеркало'),
-----DICTIONARY-3-1---------------------------------
	   (29, 5, 'address', 'адрес'),
	   (30, 5, 'bank', 'банк'),
	   (31, 5, 'bridge', 'мост'),
	   (32, 5, 'bus station', 'автовокзал'),
	   (33, 5, 'city', 'город'),
	   (34, 5, 'park', 'парк'),
	   (35, 5, 'monument', 'памятник'),
-----DICTIONARY-3-2---------------------------------
	   (36, 6, 'bus', 'автобус'),
	   (37, 6, 'car', 'машина'),
	   (38, 6, 'taxi', 'такси'),
	   (39, 6, 'tram', 'трамвай'),
	   (40, 6, 'subway', 'метро'),
	   (41, 6, 'trolleybus', 'троллейбус'),
	   (42, 6, 'driver', 'водитель'),
-----DICTIONARY-4-1---------------------------------
	   (43, 7, 'vegetables', 'овощи'),
	   (44, 7, 'cabbage', 'капуста'),
	   (45, 7, 'potatoes', 'помидор'),
	   (46, 7, 'radish', 'редис'),
	   (47, 7, 'corn', 'попкорн'),
	   (48, 7, 'pepper', 'перец'),
	   (49, 7, 'beans', 'бобы'),
-----DICTIONARY-4-2---------------------------------
	   (50, 8, 'fruit', 'фрукты'),
	   (51, 8, 'berries', 'ягоды'),
	   (52, 8, 'apple', 'яблоко'),
	   (53, 8, 'banana', 'банан'),
	   (54, 8, 'cherry', 'вишня'),
	   (55, 8, 'lemon', 'лимон'),
	   (56, 8, 'peach', 'персик');
--------------ДЛЯ СЛОВАРЯ--------------------------------------------------------
--------------ДЛЯ СЛОВАРЯ--------------------------------------------------------

