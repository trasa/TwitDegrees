SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE addFriend
	@username nvarchar(255),
	@friendname nvarchar(255),
	@location nvarchar(255) = null,
	@friendCount int = 0,
	@followerCount int = 0
AS
BEGIN
	SET NOCOUNT ON;
	-- it turns out that due to queueing, we may NOT have added the original user yet.  so do that again, first,
	-- just to be safe.
	if not exists(select name from TwitterUser where name = @username)
		exec AddTwitterUser @username, null, 0, 0
	
	-- add / update the friend user:	
	exec AddTwitterUser @friendname, @location, @friendCount, @followerCount
	
	-- add the link, if it doesn't exist.
	if not exists(select * from UserFriends where username = @username and isFriendOf = @friendName)
		INSERT INTO UserFriends (username, isFriendOf) VALUES (@username, @friendname)
END
GO
