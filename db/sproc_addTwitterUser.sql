SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE addTwitterUser
	@username nvarchar(255),
	@location nvarchar(255),
	@friendCount int = 0,
	@followerCount int = 0
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select * from TwitterUser where name = @username)
		update TwitterUser set
			location = @location,
			friendCount = @friendCount,
			followerCount = @followerCount
		where name = @username
	else
		insert into TwitterUser (name, location, friendcount, followercount)
		values (@username, @location, @friendcount, @followercount)
END
GO
