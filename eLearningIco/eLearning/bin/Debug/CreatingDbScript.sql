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

CREATE TABLE THEMES_FOR_TESTS
(
	[Theme_Id] int PRIMARY KEY IDENTITY,
	[Name_Theme] nvarchar(100) UNIQUE
)

CREATE TABLE TESTS
(
	[Test_Id] int PRIMARY KEY IDENTITY,
	[Admin_Id] int references ADMINS (Admin_Id),
	[Name_Test] nvarchar(50) UNIQUE,
	[Theme_Id] int references THEMES_FOR_TESTS (Theme_Id)
    --CONSTRAINT FK_TESTS_TO_THEMES FOREIGN KEY ([IdTheme]) REFERENCES [Themes]([IdTheme])
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

-----
GO
CREATE PROCEDURE GET_USERS AS
	SELECT * FROM USERS u LEFT JOIN ADMINS a ON a.User_Id = u.User_Id;
-----



----
GO
CREATE PROCEDURE ADD_USERS
				@login NVARCHAR(30),
				@password NVARCHAR(15)
AS
BEGIN
	INSERT INTO USERS([Login], [Password]) VALUES (@login, @password);
END
-----



-----
GO
CREATE PROCEDURE NO_PASSED_FOR_TEST 
			@user_Id int
AS
BEGIN
--�� ���������� �����
	SELECT THEMES_FOR_TESTS.Name_Theme, TESTS.Name_Test, PROGRESS_FOR_TEST.Date_Test, PROGRESS_FOR_TEST.Count_Right_Answers
		FROM PROGRESS_FOR_TEST
				JOIN TESTS ON PROGRESS_FOR_TEST.Test_Id = TESTS.Test_Id
				JOIN THEMES_FOR_TESTS ON TESTS.Theme_Id = THEMES_FOR_TESTS.Theme_Id
				WHERE PROGRESS_FOR_TEST.Is_Right = 0 AND User_Id = @user_Id;
END
-----



-----
GO
CREATE PROCEDURE PASSED_FOR_TEST 
					@user_Id int
AS
BEGIN
--���������� ����� 
SELECT THEMES_FOR_TESTS.Name_Theme, TESTS.Name_Test, PROGRESS_FOR_TEST.Date_Test, PROGRESS_FOR_TEST.Count_Right_Answers
            FROM PROGRESS_FOR_TEST
            JOIN TESTS ON PROGRESS_FOR_TEST.Test_Id = TESTS.Test_Id
            JOIN THEMES_FOR_TESTS ON TESTS.Theme_Id = THEMES_FOR_TESTS.Theme_Id
            WHERE PROGRESS_FOR_TEST.Is_Right = 1 AND User_Id =  @user_Id;
END
-----



-----
GO
CREATE PROCEDURE NO_PASSED_FOR_DICTIONARY
						@user_Id int 
AS
BEGIN
--�� ���������� ����� �� �������
SELECT * FROM PROGRESS_FOR_DICTIONARY pr
			JOIN THEMES_FOR_DICTIONARY tm ON tm.Theme_Id_Dictionary = pr.Theme_Id_Dictionary
            WHERE User_Id = @user_Id  AND Is_Right = 0;
END
------



-----
GO
CREATE PROCEDURE PASSED_FOR_DICTIONARY 
					@user_Id int 
AS
BEGIN
--���������� ����� �� �������
SELECT * FROM PROGRESS_FOR_DICTIONARY pr
			JOIN THEMES_FOR_DICTIONARY tm ON tm.Theme_Id_Dictionary = pr.Theme_Id_Dictionary
            WHERE User_Id = @user_Id AND Is_Right = 1;
END
-----



-----
GO
CREATE PROCEDURE GET_THEME_FOR_TEST AS
	SELECT * FROM THEMES_FOR_TESTS;
------




-----
GO
CREATE PROCEDURE GET_ADMINS AS
	SELECT * FROM ADMINS;
-----


-----
GO
CREATE PROCEDURE GET_TESTS_TEST AS
	SELECT * FROM TESTS;
-----



-----
GO
CREATE PROCEDURE GET_QUESTIONS AS
	SELECT * FROM QUESTIONS_FOR_TESTS;
-----



------
GO
CREATE PROCEDURE GET_TESTS_FOR_TEST 
					@theme_Id int
AS
BEGIN
	SELECT * FROM TESTS  WHERE Theme_Id = @theme_Id;
END
-----




------
GO
CREATE PROCEDURE Add_TESTS 
				@name_test NVARCHAR(50),
				@admin_id int,
				@theme_Id int
AS
BEGIN 
	INSERT INTO TESTS([Name_Test], [Admin_Id], [Theme_Id]) VALUES (@name_test, @admin_id, @theme_Id);
END
------




-----
GO
CREATE PROCEDURE Add_QUESTIONS 
					@test_Id int,
					@number_question int,
					@question NVARCHAR(100)
AS 
BEGIN
	INSERT INTO QUESTIONS_FOR_TESTS([Test_Id], [Number_Question], [Question]) VALUES (@test_Id, @number_question, @question);
END
-----




-----
GO
CREATE PROCEDURE Add_ANSWER 
				@answer NVARCHAR(50),
				@is_right bit,
				@question_Id int
AS
BEGIN 
	INSERT INTO ANSWERS_FOR_TESTS([Answer], [Is_Right], [Question_Id]) VALUES (@answer, @is_right, @question_Id);
END
-----





------
GO
CREATE PROCEDURE JOIN_QUESTION 
					@name_test NVARCHAR(50)
AS
BEGIN 
	SELECT QUESTIONS_FOR_TESTS.Question FROM QUESTIONS_FOR_TESTS
                                        JOIN TESTS ON QUESTIONS_FOR_TESTS.Test_Id = TESTS.Test_Id
                                        WHERE TESTS.Name_Test = @name_test;
END
-----



-----
GO
CREATE PROCEDURE GET_INFORMATION AS
BEGIN
	SELECT THEMES_FOR_TESTS.Name_Theme, TESTS.Name_Test, QUESTIONS_FOR_TESTS.Question FROM THEMES_FOR_TESTS
						JOIN TESTS ON THEMES_FOR_TESTS.Theme_Id = TESTS.Theme_Id
						LEFT JOIN QUESTIONS_FOR_TESTS ON TESTS.Test_Id = QUESTIONS_FOR_TESTS.Test_Id;
END
------




-------
GO
CREATE PROCEDURE DELETE_ANSWER 
					@question nvarchar (100)
AS
BEGIN
	DELETE a FROM ANSWERS_FOR_TESTS a INNER JOIN 
                                QUESTIONS_FOR_TESTS q ON a.Question_Id = q.Question_Id
                                WHERE q.Question = @question;
                           
END
-----



-----
GO
CREATE PROCEDURE DELETE_QUESTIONS 
				@question nvarchar (100)
AS
BEGIN
	DELETE FROM QUESTIONS_FOR_TESTS WHERE Question = @question;
END
-----



 
------
 GO
 CREATE PROCEDURE TEST_ARTICLES_TESTS 
						@test_Id int
 AS
 BEGIN
	SELECT TESTS.Name_Test, QUESTIONS_FOR_TESTS.Question_Id, QUESTIONS_FOR_TESTS.Number_Question, QUESTIONS_FOR_TESTS.Question FROM QUESTIONS_FOR_TESTS
            join TESTS ON QUESTIONS_FOR_TESTS.Test_Id = TESTS.Test_Id
            where TESTS.Test_Id = @test_Id;
END
------





------
GO
CREATE PROCEDURE TEST_ARTICLES_ANSWERS 
					@question NVARCHAR(100)
AS
BEGIN
	SELECT ANSWERS_FOR_TESTS.Answer FROM ANSWERS_FOR_TESTS
                                               JOIN QUESTIONS_FOR_TESTS on ANSWERS_FOR_TESTS.Question_Id = QUESTIONS_FOR_TESTS.Question_Id
                                                where QUESTIONS_FOR_TESTS.Question = @question;
END
------




-----
GO 
CREATE PROCEDURE TEST_ARTICLES_QUESTIONS 
					@name_test NVARCHAR(50)
AS
BEGIN
	SELECT QUESTIONS_FOR_TESTS.Number_Question, QUESTIONS_FOR_TESTS.Question, ANSWERS_FOR_TESTS.Answer FROM QUESTIONS_FOR_TESTS
        JOIN ANSWERS_FOR_TESTS ON QUESTIONS_FOR_TESTS.Question_Id = ANSWERS_FOR_TESTS.Question_Id
        JOIN TESTS ON QUESTIONS_FOR_TESTS.Test_Id = TESTS.Test_Id
        WHERE ANSWERS_FOR_TESTS.Is_Right = 1 AND TESTS.Name_Test = @name_test;
END
-----





-----
GO
--���� ������� 
CREATE PROCEDURE ADD_PROGRESS_FOR_TESTS 
					@user_Id int,
					@test_Id int,
					@date_test nvarchar(50),
					@is_right bit,
					@count_right_answer int
AS
BEGIN
	INSERT INTO PROGRESS_FOR_TEST(User_Id, Test_Id, Date_Test, Is_Right, Count_Right_Answers) 
						VALUES (@user_Id, @test_Id, @date_test, 1, @count_right_answer);
END
-----





------
GO
--���� �� �������
CREATE PROCEDURE NO_ADD_PROGRESS_FOR_TESTS 
					@user_Id int,
					@test_Id int,
					@date_test nvarchar(50),
					@is_right bit,
					@count_right_answer int 
AS
BEGIN
	INSERT INTO PROGRESS_FOR_TEST(User_Id, Test_Id,  Date_Test, Is_Right, Count_Right_Answers) 
						VALUES (@user_Id, @test_Id, @date_test, 0, @count_right_answer);
END
---------




-----
GO
CREATE PROCEDURE GET_THEME_FOR_DICTIONARY AS
BEGIN
	SELECT * FROM THEMES_FOR_DICTIONARY;
END
------



------
GO
CREATE PROCEDURE GET_POD_THEMES AS
BEGIN
	SELECT * FROM POD_THEMES;
END
------





-------
GO 
CREATE PROCEDURE TEST_DICTIONARY
					@theme_id int,
					@podtheme_id int
AS
BEGIN
	SELECT Words.Word_Id, Words.English_Word, Words.Russian_Word
                FROM Words
                JOIN POD_THEMES ON Words.Pod_Theme_Id = POD_THEMES.Pod_Theme_Id
                LEFT JOIN THEMES_FOR_DICTIONARY ON THEMES_FOR_DICTIONARY.Theme_Id_Dictionary = Pod_Themes.Theme_Id_Dictionary
                WHERE THEMES_FOR_DICTIONARY.Theme_Id_Dictionary = @theme_id
						AND POD_THEMES.Pod_Theme_Id = @podtheme_id;
END
-------


-- ���������� ��� ��� ������
INSERT INTO [THEMES_FOR_TESTS]
	VALUES 	('�������'),
			('�������'),
			('��������������'),
			('���������������');


--���������� ���� � �������
INSERT INTO [POD_THEMES] ([Pod_Theme_Id], [Theme_Id_Dictionary], [Pod_Theme_Name])
VALUES (1, 1, '���������� � ��������'),
	   (2, 1, '������� ����������'),
	   (3, 2, '���, ��������'),
	   (4, 2, '������, ��������'),
	   (5, 3, '�����'),
	   (6, 3, '���������'),
	   (7, 4, '�����'),
	   (8, 4, '������');

INSERT INTO [Words] ([Word_Id], [Pod_Theme_Id], [English_Word], [Russian_Word])
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
	   (45, 7, 'potato', '��������'),
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


--------
GO
--�������� �� �������
CREATE PROCEDURE ADD_PROGRESS_FOR_DICTIONARY 
							@user_Id int,
							@theme_Id_Dictionary int,
							@date_test NVARCHAR(50),
							@is_right bit,
							@count_right_answer int,
							@count_question int
AS
BEGIN
	INSERT INTO PROGRESS_FOR_DICTIONARY(User_Id, Theme_Id_Dictionary ,Date_Test, Is_Right, Count_Right_Answers, Count_Question)
                        VALUES (@user_Id,@theme_Id_Dictionary , @date_test, @is_right, @count_right_answer, @count_question );
					
END
-----




---------

--Import to xml
GO
CREATE PROCEDURE Import_To_XML
AS
BEGIN
	INSERT INTO TESTS(Admin_Id, Name_Test, Theme_Id)
		SELECT
		   MY_XML.Test.query('Admin_Id').value('.', 'int'),
		   MY_XML.Test.query('Name_Test').value('.', 'NVARCHAR(50)'),
		   MY_XML.Test.query('Theme_Id').value('.', 'int')
   
		FROM (SELECT CAST(MY_XML AS xml)
			  FROM OPENROWSET(BULK 'D:\�����\3 curs\DB\��\Tests.xml', SINGLE_BLOB) AS T(MY_XML)) AS T(MY_XML)
			  CROSS APPLY MY_XML.nodes('Tests/Test') AS MY_XML (Test);
END



--Export to xml
GO
CREATE PROCEDURE Export_To_XML
AS
BEGIN
	SELECT * FROM TESTS
		for XML PATH('Test'), Root('Tests');
END

----------
GO

--�������� �������������
CREATE LOGIN [Admin_User]
    WITH PASSWORD=N'password',
    DEFAULT_DATABASE=[eLEARNING],
    DEFAULT_LANGUAGE=[�������],
    CHECK_EXPIRATION=OFF,
    CHECK_POLICY=ON;

CREATE LOGIN [Default_User]
    WITH PASSWORD=N'password',
    DEFAULT_DATABASE=[eLEARNING],
    DEFAULT_LANGUAGE=[�������],
    CHECK_EXPIRATION=OFF,
    CHECK_POLICY=ON;


GO
CREATE USER [Admin_User] FOR LOGIN [Admin_User] WITH DEFAULT_SCHEMA=[dbo];
CREATE USER [Default_User] FOR LOGIN [Default_User] WITH DEFAULT_SCHEMA=[dbo];
GO

CREATE ROLE ADMIN_ROLE AUTHORIZATION db_owner;
CREATE ROLE USER_ROLE AUTHORIZATION db_owner;

-- ������ �����
GRANT EXECUTE TO [ADMIN_ROLE];
GRANT SELECT TO [ADMIN_ROLE];
GRANT DELETE TO [ADMIN_ROLE];
GRANT INSERT TO [ADMIN_ROLE];
GRANT ALTER TO [ADMIN_ROLE];
GRANT UPDATE TO [ADMIN_ROLE];


GRANT EXECUTE TO [USER_ROLE];
GRANT SELECT TO [USER_ROLE];

ALTER ROLE ADMIN_ROLE ADD MEMBER [Admin_User];
ALTER ROLE USER_ROLE ADD MEMBER [Default_User];

GO

--------
CREATE TRIGGER Question_Removed
ON QUESTIONS_FOR_TESTS AFTER DELETE
AS
BEGIN
	DELETE p FROM PROGRESS_FOR_TEST p
		WHERE 0 = (SELECT COUNT(*) FROM QUESTIONS_FOR_TESTS q WHERE q.Test_Id = p.Test_Id);

	DELETE t FROM TESTS t
		WHERE 0 = (SELECT COUNT(*) FROM QUESTIONS_FOR_TESTS q WHERE q.Test_Id = t.Test_Id);
END
-----------------------------------------------------------------------------------------------------------------------------------------------------

GO
USE master;

-- ���� ������ ����� �� ���������� ��������� ������� �� xml �����
GRANT ADMINISTER BULK OPERATIONS TO [ADMIN_ROLE];

-- ��������� ������ ��� ����������� �������������� � ������� ������ � ������
EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', 
                          N'Software\Microsoft\MSSQLServer\MSSQLServer',      
                          N'LoginMode', REG_DWORD, 1
GO
