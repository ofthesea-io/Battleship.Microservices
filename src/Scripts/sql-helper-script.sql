SELECT  p.PlayerId, p.Firstname, p.Lastname, P.Email, p.IsDemo, p.DateCreated from [Battleship.Player].dbo.Player p
SELECT * FROM [Battleship.Game].dbo.GamePlay
SELECT * FROM [Battleship.ScoreCard].dbo.PlayerScoreCard psc 

DELETE FROM [Battleship.Game].dbo.GamePlay
DELETE FROM [Battleship.ScoreCard].dbo.PlayerScoreCard
DELETE FROM [Battleship.Player].dbo.Player WHERE [Battleship.Player].dbo.Player.IsDemo = 0
