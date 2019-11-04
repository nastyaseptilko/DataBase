CREATE DATABASE eLEARNING;
 USE eLEARNING;

CREATE TABLE USERS(
	[User_Id] int PRIMARY KEY IDENTITY,
	[Login] nvarchar(30) NOT NULL,
	[Password] nvarchar(15) NOT NULL
)

CREATE TABLE ADMINS(
	[Admin_Id] int PRIMARY KEY IDENTITY,
	[User_Id] int references USERS (User_Id)
)

CREATE TABLE PROGRESS_FOR_TEST
(
	[Progress_Id] int PRIMARY KEY IDENTITY,
	[User_Id] int references USERS (User_Id),
	[Test_Id] int references TESTS (Test_Id),
	[Date_Test] nvarchar(50),
	[Is_Right] bit,
	[Count_Right_Answers] int
	--CONSTRAINT FK_PROGRESS_TO_USERS FOREIGN KEY ([User_Id]) REFERENCES [Users]([IdUser]),
	--CONSTRAINT FK_PROGRESS_TO_TESTS FOREIGN KEY ([IdTest]) REFERENCES [Tests]([IdTest])
) 

CREATE TABLE TESTS
(
	[Test_Id] int PRIMARY KEY IDENTITY,
	[Admin_Id] int references ADMINS (Admin_Id),
	[Name_Test] nvarchar(50) UNIQUE,
	[Theme_Id] int references THEMES_FOR_TESTS (Theme_Id)
    --CONSTRAINT FK_TESTS_TO_THEMES FOREIGN KEY ([IdTheme]) REFERENCES [Themes]([IdTheme])
)

CREATE TABLE THEMES_FOR_TESTS
(
	[Theme_Id] int PRIMARY KEY IDENTITY,
	[Name_Theme] nvarchar(100) UNIQUE
)

CREATE TABLE QUESTIONS_FOR_TESTS
(
	[Question_Id] int PRIMARY KEY IDENTITY,
	[Test_Id] int references TESTS (Test_Id),
	[Number_Question] int,
	[Question] nvarchar(100) UNIQUE
	--CONSTRAINT FK_QUESTIONS_TO_TESTS FOREIGN KEY ([IdTest]) REFERENCES [Tests]([IdTest])
)

CREATE TABLE ANSWERS_FOR_TESTS
(
	[Answer_Id] int PRIMARY KEY IDENTITY,
	[Answer] nvarchar(50),
	[Is_Right] bit,
	[Question_Id] int references QUESTIONS_FOR_TESTS (Question_Id) ,
	--CONSTRAINT FK_ANSWER_TO_QUESTIONS FOREIGN KEY ([IdQuestion]) REFERENCES [Questions]([IdQuestion]),
)

CREATE TABLE PROGRESS_FOR_DICTIONARY
(
	[Progress_Id] int PRIMARY KEY IDENTITY,
	[User_Id] int references USERS (User_Id),
	[Theme_Id_Dictionary] int references THEMES_FOR_DICTIONARY (Theme_Id_Dictionary),
	[Date_Test] nvarchar(50),
	[Is_Right] bit,
	[Count_Right_Answers] int,
	[Count_Question] int
	--CONSTRAINT FK_TESTDICTIONATY_TO_USERS FOREIGN KEY ([IdUser]) REFERENCES [Users]([IdUser]),
)

CREATE TABLE THEMES_FOR_DICTIONARY
(
	[Theme_Id_Dictionary] int PRIMARY KEY,
	[Admin_Id] int references ADMINS (Admin_Id),
	[Name_Theme_For_Dictionary] nvarchar(50)
)

CREATE TABLE POD_THEMES
(
	[Pod_Theme_Id] int PRIMARY KEY,
	[Theme_Id_Dictionary] int references THEMES_FOR_DICTIONARY (Theme_Id_Dictionary),
	[Pod_Theme_Name] nvarchar(50)
	--CONSTRAINT FK_PODTHEMES_TO_THEMESFORDICTIONARY FOREIGN KEY ([IdThemeForDictionary]) REFERENCES [ThemesForDictionary]([IdTheme])
)

CREATE TABLE WORDS
(
	[Word_Id] int PRIMARY KEY,
	[Pod_Theme_Id] int references POD_THEMES (Pod_Theme_Id),
	[English_Word] nvarchar(30),
	[Russian_Word] nvarchar(30)
)
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

USE eLEARNING;
GO
CREATE PROCEDURE Get_USERS AS
SELECT * FROM USERS
EXEC Get_USERS;



GO
CREATE PROCEDURE Add_USERS AS
INSERT INTO USERS([Login], [Password]) VALUES ('{txbLogin.Text}', '{txbPassword1.Password}')
SELECT * FROM USERS
EXEC Add_USERS;


GO
CREATE PROCEDURE NO_PASSED_FOR_TEST AS
--�� ���������� �����
 SELECT THEMES_FOR_TESTS.Name_Theme, PROGRESS_FOR_TEST.Test_Id, PROGRESS_FOR_TEST.Date_Test, PROGRESS_FOR_TEST.Count_Right_Answers
            FROM PROGRESS_FOR_TEST
            JOIN TESTS ON PROGRESS_FOR_TEST.Test_Id = TESTS.Test_Id
            JOIN THEMES_FOR_TESTS ON TESTS.Theme_Id = THEMES_FOR_TESTS.Theme_Id
            WHERE PROGRESS_FOR_TEST.Is_Right = 1 AND User_Id = {user.idUser};
EXEC NO_PASSED_FOR_TEST;


GO
CREATE PROCEDURE PASSED_FOR_TEST AS
--���������� ����� 
SELECT THEMES_FOR_TESTS.Name_Theme, PROGRESS_FOR_TEST.Test_Id, PROGRESS_FOR_TEST.Date_Test, PROGRESS_FOR_TEST.Count_Right_Answers
            FROM PROGRESS_FOR_TEST
            JOIN TESTS ON PROGRESS_FOR_TEST.Test_Id = TESTS.Test_Id
            JOIN THEMES_FOR_TESTS ON TESTS.Theme_Id = THEMES_FOR_TESTS.Theme_Id
            WHERE PROGRESS_FOR_TEST.Is_Right = 0 AND User_Id = {user.idUser};
EXEC PASSED_FOR_TEST;


GO
CREATE PROCEDURE NO_PASSED_FOR_DICTIONARY AS
--�� ���������� ����� �� �������
SELECT * FROM PROGRESS_FOR_DICTIONARY
            WHERE User_Id = {user.idUser} AND IsRight = 0;
EXEC NO_PASSED_FOR_DICTIONARY;


GO
CREATE PROCEDURE PASSED_FOR_DICTIONARY AS
--���������� ����� �� �������
SELECT * FROM PROGRESS_FOR_DICTIONARY
            WHERE User_Id = {user.idUser} AND IsRight = 1;
EXEC PASSED_FOR_DICTIONARY;



GO 
CREATE PROCEDURE CREATE_TESTS AS
SELECT THEMES_FOR_TESTS.Name_Theme, TESTS.Name_Test FROM TESTS
                                       JOIN THEMES_FOR_TESTS ON TESTS.Theme_Id = THEMES_FOR_TESTS.Theme_Id
                                       WHERE THEMES_FOR_TESTS.Name_Theme = '{listThemes.SelectedItem.ToString()}';
EXEC CREATE_TESTS;


GO
CREATE PROCEDURE Add_THEME AS 
INSERT INTO THEMES_FOR_TESTS([Name_Theme]) VALUES ('{themeInBD.NameTheme.ToString()}');
EXEC Add_THEME;



GO
CREATE PROCEDURE Add_TESTS AS 
INSERT INTO TESTS([Name_Test], [Theme_Id]) VALUES ('{testInBD.Name}', {theme.IdTheme});
EXEC  Add_TESTS;


GO
CREATE PROCEDURE Add_QUESTIONS AS 
INSERT INTO QUESTIONS_FOR_TESTS([Test_Id], [Number_Question], [Question]) VALUES ({test.idTest}, {numberQuestion}, '{questionInDB.SomeQuestion}');
EXEC Add_QUESTIONS;



GO
CREATE PROCEDURE Add_ANSWER AS 
INSERT INTO ANSWERS_FOR_TESTS([Answer], [Is_Right], [Question_Id]) VALUES ('{listAnswerInDB[0].SomeAnswer}', 1, {question.IdQuestion});
EXEC Add_ANSWER;



GO
CREATE PROCEDURE CREATE_QUESTION AS 
SELECT QUESTIONS_FOR_TESTS.Question FROM QUESTIONS_FOR_TESTS
                                        JOIN TESTS ON QUESTIONS_FOR_TESTS.Test_Id = TESTS.Test_Id
                                        WHERE TESTS.Name_Test = '{listTests.SelectedItem}';
EXEC CREATE_QUESTION;