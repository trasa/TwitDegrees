select	AllFriends.IsFriendOf, 
		COUNT(allFriends.IsFriendOf) as CommonFriendsCount,
		u.FollowerCount,
		dbo.getTotalFollowerCount(AllFriends.IsFriendOf) as DataFollowerCount
from UserFriends as AllFriends 
	inner join UserFriends friendsOfCarroll on AllFriends.UserName = friendsOfCarroll.UserName
	inner join TwitterUser u on u.Name = AllFriends.IsFriendOf
where friendsOfCarroll.IsFriendOf = 'petecarroll'
group by AllFriends.IsFriendOf, u.followercount
having COUNT(allFriends.IsFriendOf) > 100 
order by COUNT(allFriends.IsFriendOf) desc

