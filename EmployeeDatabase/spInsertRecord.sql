
ALTER procedure [dbo].[spInsertRecord]
(@FirstName varchar(20),@LastName varchar(20),@Gender varchar(10),@EmailId varchar(50),@PhoneNumber varchar(10),@City varchar(10))
As
Begin
	DECLARE @status int
	set @status=0
	
		IF NOT EXISTS (SELECT * FROM [dbo].[EmployeeDatabase] WHERE [FirstName] = @FirstName AND [LastName] = @LastName AND [EmailId] = @EmailId)
		Begin
			BEGIN TRANSACTION  
			insert into EmployeeDatabase(FirstName,LastName,Gender,EmailId,PhoneNumber,City,RegistrationDate)
			values(@FirstName,@LastName,@Gender,@EmailId,@PhoneNumber,@City,CURRENT_TIMESTAMP)
			COMMIT 
			SELECT EmployeeId,FirstName,LastName,Gender,EmailId,PhoneNumber,City,RegistrationDate FROM EmployeeDatabase where EmailId = @EmailId;
		End
		else
		Begin
			BEGIN TRANSACTION  
			insert into EmployeeDatabase(FirstName,LastName,Gender,EmailId,PhoneNumber,City,RegistrationDate)
			values(@FirstName,@LastName,@Gender,@EmailId,@PhoneNumber,@City,CURRENT_TIMESTAMP)
			Rollback
			Select @status
		End
End