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
VALUES ('�������'), ('�������'), ('��������������'), ('���������������');

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


--------------��� �������-----------------------------------------------------
--------------��� �������--------------------------------------------------------

INSERT INTO [ThemesForDictionary] ([IdTheme], [NameThemeForDictionary])
VALUES (1, '����������. �������'),
	   (2, '���'),
	   (3, '�����'),
	   (4, '���');

INSERT INTO [PodThemes] ([IdTheme], [IdThemeForDictionary], [NameTheme])
VALUES (1, 1, '���������� � ��������'),
	   (2, 1, '������� ����������'),
	   (3, 2, '���, ��������'),
	   (4, 2, '������, ��������'),
	   (5, 3, '�����'),
	   (6, 3, '���������'),
	   (7, 4, '�����'),
	   (8, 4, '������');

INSERT INTO [Words] ([IdWord], [IdTheme], [EnglishWord], [RussianWord])
VALUES 
-----DICTIONARY-1-1---------------------------------
	   (1, 1, 'Hello', '������������'),
	   (2, 1, 'Hi', '������'),
       (3, 1, 'Goodbye', '�� ��������'),
	   (4, 1, 'Bye', '����'),
	   (5, 1, 'Good morning', '������ ����'),
	   (6, 1, 'See you later', '�� �������'),
	   (7, 1, 'Good night', '��������� ����'),
-----DICTIONARY-1-2---------------------------------
       (8, 2, 'Thank you', '�������'),
	   (9, 2, 'Thanks a lot', '������� �������'),
	   (10, 2, 'You are welcome', '����������'),
	   (11, 2, 'Not at all', '�� �� ���'),
	   (12, 2, 'No problem', '��� �������'),
	   (13, 2, 'Excuse me', '��������'),
	   (14, 2, 'Thats OK', '�� ���������'),
-----DICTIONARY-2-1---------------------------------
       (15, 3, 'house', '���'),
	   (16, 3, 'flat', '��������'),
	   (17, 3, 'door', '�����'),
	   (18, 3, 'floor', '���'),
	   (19, 3, 'wall', '�����'),
	   (20, 3, 'window', '����'),
	   (21, 3, 'bedroom', '�������'),
-----DICTIONARY-2-2---------------------------------
       (22, 4, 'bed', '�������'),
	   (23, 4, 'chair', '����'),
	   (24, 4, 'table', '����'),
	   (25, 4, 'lamp', '�����'),
	   (26, 4, 'picture', '�������'),
	   (27, 4, 'carpet', '����'),
	   (28, 4, 'mirror', '�������'),
-----DICTIONARY-3-1---------------------------------
	   (29, 5, 'address', '�����'),
	   (30, 5, 'bank', '����'),
	   (31, 5, 'bridge', '����'),
	   (32, 5, 'bus station', '����������'),
	   (33, 5, 'city', '�����'),
	   (34, 5, 'park', '����'),
	   (35, 5, 'monument', '��������'),
-----DICTIONARY-3-2---------------------------------
	   (36, 6, 'bus', '�������'),
	   (37, 6, 'car', '������'),
	   (38, 6, 'taxi', '�����'),
	   (39, 6, 'tram', '�������'),
	   (40, 6, 'subway', '�����'),
	   (41, 6, 'trolleybus', '����������'),
	   (42, 6, 'driver', '��������'),
-----DICTIONARY-4-1---------------------------------
	   (43, 7, 'vegetables', '�����'),
	   (44, 7, 'cabbage', '�������'),
	   (45, 7, 'potatoes', '�������'),
	   (46, 7, 'radish', '�����'),
	   (47, 7, 'corn', '�������'),
	   (48, 7, 'pepper', '�����'),
	   (49, 7, 'beans', '����'),
-----DICTIONARY-4-2---------------------------------
	   (50, 8, 'fruit', '������'),
	   (51, 8, 'berries', '�����'),
	   (52, 8, 'apple', '������'),
	   (53, 8, 'banana', '�����'),
	   (54, 8, 'cherry', '�����'),
	   (55, 8, 'lemon', '�����'),
	   (56, 8, 'peach', '������');
--------------��� �������--------------------------------------------------------
--------------��� �������--------------------------------------------------------

