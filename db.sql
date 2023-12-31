USE [TIENDA]
GO
/****** Object:  Table [dbo].[BOOKS]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BOOKS](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[price] [decimal](10, 2) NULL,
	[stock] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ITEM]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ITEM](
	[fkIdUser] [int] NOT NULL,
	[fkIdBooks] [int] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[fkIdUser] ASC,
	[fkIdBooks] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[id] [int] NOT NULL,
	[name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BOOKS] ON 

INSERT [dbo].[BOOKS] ([id], [name], [price], [stock]) VALUES (2, N'book name', CAST(10.50 AS Decimal(10, 2)), 10)
INSERT [dbo].[BOOKS] ([id], [name], [price], [stock]) VALUES (3, N'testBook', CAST(20000.00 AS Decimal(10, 2)), 25)
SET IDENTITY_INSERT [dbo].[BOOKS] OFF
GO
ALTER TABLE [dbo].[ITEM] ADD  DEFAULT ((1)) FOR [quantity]
GO
ALTER TABLE [dbo].[ITEM]  WITH CHECK ADD FOREIGN KEY([fkIdBooks])
REFERENCES [dbo].[BOOKS] ([id])
GO
ALTER TABLE [dbo].[ITEM]  WITH CHECK ADD FOREIGN KEY([fkIdUser])
REFERENCES [dbo].[USERS] ([id])
GO
/****** Object:  StoredProcedure [dbo].[usp_AddBook]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_AddBook]
@name VARCHAR(50), @price DECIMAL(10,2), @stock INT
AS
BEGIN
	INSERT INTO BOOKS
	(name,price,stock)
	VALUES
	(@name, @price, @stock)
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_CreateUser]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROCEDURE [dbo].[usp_CreateUser]
 @id INT,
 @name VARCHAR(50)
 AS
 BEGIN
 INSERT INTO USERS VALUES(@id, @name)
 END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteBook]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_DeleteBook]
@id int
AS
BEGIN
DELETE FROM Books WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteUser]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE PROCEDURE [dbo].[usp_DeleteUser]
  @Id INT
  AS
  BEGIN
  DELETE FROM USERS WHERE Id = @Id
  END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetBook]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_GetBook]
@id INT
AS
	SELECT * FROM BOOKS WHERE id = @id;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUser]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 CREATE PROC [dbo].[usp_GetUser]
@Id INT 
AS
	SELECT * FROM USERS WHERE id = @Id
GO
/****** Object:  StoredProcedure [dbo].[usp_ListBooks]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_ListBooks]
AS
	SELECT * FROM BOOKS
GO
/****** Object:  StoredProcedure [dbo].[usp_ListUsers]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
   CREATE PROCEDURE [dbo].[usp_ListUsers]
 AS
 SELECT * FROM USERS
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateBook]    Script Date: 28/11/2023 04:23:13 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[usp_UpdateBook]
@id INT, @name VARCHAR(50) = null, @price DECIMAL(10,2) = null, @stock INT= null
AS
BEGIN
	UPDATE BOOKS SET
	name = ISNULL(@name, name),
	price = ISNULL (@price, price),
	stock = ISNULL (@stock, stock)
	WHERE id = @id
END;
GO
