DELETE FROM [Battleship.Game].dbo.GamePlay;
DELETE FROM [Battleship.ScoreCard].dbo.PlayerScoreCard;
DELETE FROM [Battleship.Player].dbo.Player WHERE IsDemo = 0;