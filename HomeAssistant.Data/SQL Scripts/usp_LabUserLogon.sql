USE [dbLims]
GO
/****** Object:  StoredProcedure [dbo].[usp_LabUserLogon]    Script Date: 25.08.2017 10:34:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_LabUserLogon]
(
    @szUser           NVARCHAR (30)
  , @szPassword      NVARCHAR (30)  
  
)
AS
BEGIN
 SET ROWCOUNT 0
  SET NOCOUNT ON 

  DECLARE @szError			NVARCHAR(255)

  SELECT TOP (1) from tblUser where 

  declare @t table( hUser int
				  , szUser nvarchar(32)
				  , szFullName nvarchar(30)
				  , szPassword nvarchar(30)
				  , nPwdVersion int
				  , tLastModified int
				  , bAccess BIT
				  , nAccessMsgCode INT
				  ,	szUserGroup NVARCHAR(32)
				  ,	szComputerGroup NVARCHAR(32)
				  ,	rc INT,	szMsg NVARCHAR(255)
				  ,	nUserType INT
				  ,	szCheckUserName NVARCHAR(80))
  
  SELECT @User = LTRIM(RTRIM(@szUser)), @Pass = RTRIM(@szPassword), @tDatetimeNow = GETDATE()

  
  insert  into @t
	  exec [dbIdc].[dbo].[sp_IdcUserLogon] @User
										  ,@Pass
										  ,N''
										  ,N''
										  ,N''
										  ,N''
										  ,1
										  ,0
										  ,0
										  ,NULL
										  ,NULL
										  ,NULL
										  ,NULL 
 


 IF NOT EXISTS (SELECT nKey FROM [dbo].[tbl_UserLoginCount] WHERE szUser = @User)
		INSERT INTO [dbo].[tbl_UserLoginCount]
					SELECT   [szUser] = @User
							,[nCount] = 0
							,[bLocked] = 0
							,NULL
							,[tLastCreated] = @tDatetimeNow
							,NULL

SELECT @nCountSet = [nParamValue], @szCountSet = [nParamValue] FROM [dbIdc].[dbo].[tblIdcSysParam] WHERE [nGroupLink] = 4 AND RTRIM([szName]) = 'nLoginAttempts'
SELECT @tLockTimeSet = DATEADD(mi,[nParamValue],'1900-01-01 00:00:00.000'), @szlockTimeSet = [nParamValue], @nLockTimeSet = [nParamValue] FROM [dbIdc].[dbo].[tblIdcSysParam] WHERE [nGroupLink] = 4 AND RTRIM([szName]) = 'nLoginDelayTime'
SELECT @nUserCountLink = nKey, @nCountAct = [nCount], @bLocked = [bLocked], @tLockTimeAct = [tLastWrong], @tLockTimeNow = [tLastWrong] FROM [dbo].[tbl_UserLoginCount] WHERE [szUser] = @User

SELECT @szError =	CASE nAccessMsgCode
						WHEN 0 THEN NULL
						WHEN -1037040659 THEN N'Пользователь не существует в системе или введен неверный пароль'
						WHEN -1037040656 THEN N'Пользователь заблокирован администратором'
						WHEN -1037040658 THEN N'Компьютер находится в группе, для входа в которую доступ пользователю запрещён'
					END
FROM @t


IF @nCountSet != 0 AND @nLockTimeSet != 0
BEGIN
	IF (SELECT nAccessMsgCode FROM @t) = 0
		BEGIN
			IF @bLocked = 1 AND (@tDatetimeNow - @tLockTimeAct) > @tLockTimeSet
				SELECT @bLocked = 0, @nCountAct = 0
			ELSE
				SELECT @nCountAct = 0
		END
	ELSE
		BEGIN
			IF @bLocked = 1 AND (@tDatetimeNow - @tLockTimeAct) > @tLockTimeSet
				SELECT @nCountAct = 0, @tLockTimeNow = @tDatetimeNow, @bLocked = 0
			SELECT @nCountAct = @nCountAct + 1
			IF @nCountAct >= @nCountSet
				SELECT @bLocked = 1
			IF @bLocked = 1
				SELECT @tLockTimeNow = @tLockTimeAct
			ELSE
				SELECT  @tLockTimeNow = @tDatetimeNow
		END

	UPDATE [dbo].[tbl_UserLoginCount]
			SET  [nCount] = @nCountAct
				,[bLocked] = @bLocked
				,[tLastWrong] = @tLockTimeNow
				,[tLastUpdated] = @tDatetimeNow 
			WHERE  nKey = @nUserCountLink	
 
	IF @bLocked = 1
		BEGIN
			SELECT @szLockTimeLost = CONVERT(TIME,(@tLockTimeSet - (@tDatetimeNow - @tLockTimeAct)))
			SELECT @szError = N'Пользователь заблокирован на ' + @szlockTimeSet + N' минут, превышено количество(' + @szCountSet + N') вводов неправильного пароля. Повторный вход возможен, через ' + @szLockTimeLost
		END
END
IF @szError IS NOT NULL
	RAISERROR (@szError,16,1) 
ELSE
	select * from @t
  
END
 

