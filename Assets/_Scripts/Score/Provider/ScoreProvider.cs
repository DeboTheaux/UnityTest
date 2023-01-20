namespace UT.GameLogic
{
    public static class ScoreProvider
    {
        private static IScoreService _scoreService;

        //We can toggle between remote or local service later...
        public static IScoreService GetScoreService
        {
            //_scoreService ??= new LocalScoreService(); 
            get
            {
                if (_scoreService is null)
                    return _scoreService = new LocalScoreService();
                else
                    return _scoreService;
            }
        }
    }
}
