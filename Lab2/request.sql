//1.6
USE TradeDB
SELECT * FROM Trading_enterprisessUkraine

//1.7
use TradeDB
select Trading_enterprisessUkraine.AmountOfTrade_ID, Trading_enterprisessUkraine.EnterpriseName, Amount_of_trade.Description
from Trading_enterprisessUkraine 
inner join Amount_of_trade on Amount_of_trade.ID = Trading_enterprisessUkraine.AmountOfTrade_ID
 
//1.8
use TradeDB
select Trading_enterprisessUkraine.AmountOfTrade_ID, Trading_enterprisessUkraine.EnterpriseName, Amount_of_trade.Description
from Trading_enterprisessUkraine 
inner join Amount_of_trade on Amount_of_trade.ID = Trading_enterprisessUkraine.AmountOfTrade_ID 
where Amount_of_trade.Description = 'food'

//1.9
use TradeDB
select EnterpriseName,EnterpriseOwner from Trading_enterprisessUkraine
where ID=9

//1.10
use TradeDB
insert into Trading_enterprisessUkraine (EnterpriseName,EnterpriseOwner,Type,Price,AmountOfTrade_ID)
values ('Microsoft','Gates','LLC',18,1)

//1.11
use TradeDB
insert into Amount_of_trade (Description, Money)
values ('waepoons',458)

//1.12
use TradeDB
update Trading_enterprisessUkraine set
EnterpriseName='company', EnterpriseOwner='master', Type='TopSecret', Price=18, AmountOfTrade_ID=2
where ID=13

//1.13
use TradeDB
delete Trading_enterprisessUkraine
where ID=10
  

//creating table 

create table Trading_enterprisessUkraine
(
    ID INT  AUTO_INCREMENT PRIMARY KEY,
    EnterpriseName NVARCHAR(50) NOT NULL,
    EnterpriseOwner NVARCHAR(50) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Price real NOT NULL,
    AmountOfTrade_ID int NOT NULL,
    FOREIGN KEY(AmountOfTrade_ID) REFERENCES Amount_of_trade(ID_am)
);


create table Amount_of_trade
(
    ID_am INT AUTO_INCREMENT PRIMARY KEY,
    Money real NOT NULL,
    Description NVARCHAR(50) NOT NULL
);

//adding foreign KEY

ALTER TABLE Trading_enterprisessUkraine
ADD FOREIGN KEY(AmountOfTrade_ID) REFERENCES Amount_of_trade(ID) ON DELETE CASCADE;