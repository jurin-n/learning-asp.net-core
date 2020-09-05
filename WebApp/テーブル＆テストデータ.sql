/*
user01ユーザでSQL Server認証して行う作業
*/
USE dev2
GO
--テーブル作成
CREATE TABLE [dbo].[Users](
    [UserID] [nvarchar](32) NOT NULL,  
    [Name] [nvarchar](50) NULL,  
    [Password] [nvarchar](256) NULL,  
    [CreatedOn] [datetime] NOT NULL
CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED   
(  
[UserID] ASC  
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]  
GO

/*　データ登録 */
USE dev2
GO
--データ登録
INSERT INTO Users (UserID,Name,Password,CreatedOn) Values('test-user','テストユーザ','test-password','2020-07-24');


/*
セッション用
*/
dotnet tool install --global dotnet-sql-cache
dotnet sql-cache create "Data Source=localhost;Initial Catalog=dev2;Integrated Security=True;" dbo AppCache

