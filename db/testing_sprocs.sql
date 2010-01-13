exec addTwitterUser 'test', null, null, null

exec addTwitterUser 'test', 'lol',5, 6

--delete userfriends where isfriendof = 'test' or username = 'test'
--delete twitteruser where name in ('test', 'friend1', 'friend2')

exec addFriend 'trasa', 'test', 'location', 5, 560


select * from twitteruser where name in ('test', 'friend1', 'friend2')
select * from userfriends where username = 'test'
select * from userfriends where isfriendof = 'test'
