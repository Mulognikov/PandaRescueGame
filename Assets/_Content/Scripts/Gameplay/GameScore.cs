using Zenject;

public class GameScore : ITickable
{
    public int Stars
    {
        get => _currentStars;
        private set
        {
            _currentStars = value;
            if (_currentStars < 0) _currentStars = 0;
        }
    }
    
    //private WritableGameScoreModel _gameScore;
    private GameStateModel _gameState;
    private GameSettings _gameSettings;
    private Line _line;

    private int _currentStars;

    public GameScore(/*GameScoreModel gameScore,*/ GameStateModel gameState, GameSettings gameSettings, Line line)
    {
        //_gameScore = (WritableGameScoreModel)gameScore;
        _gameState = gameState;
        _gameSettings = gameSettings;
        _line = line;

        Stars = 3;
    }
    
    public void Tick()
    {
        UpdateStars();
    }

    private void UpdateStars()
    {
        if (_gameState.CurrentGameState == GameStateEnum.Lose)
        {
            Stars = 0;
            return;
        }
        
        if (_line.CurrentLineLength > _gameSettings.ThreeStarsLength)   Stars = 2;
        if (_line.CurrentLineLength > _gameSettings.TwoStarsLenght)     Stars = 1;
    }

}
