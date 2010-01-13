select count(1) from twitteruser -- order by friendcount
select count(1) from userfriends -- where username = 'trasa'

select * from twitteruser where name = 'trasa'
select * from userfriends where username = 'trasa'
select * from userfriends where isfriendof = 'trasa'

--sp_help twitteruser