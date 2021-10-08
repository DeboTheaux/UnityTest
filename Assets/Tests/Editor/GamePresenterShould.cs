using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

public class GamePresenterShould
{
    private const int Once = 1;

    [Test]
    public void InitializeSpawnersWhenPresenting()
    {
        var view = AView;
        var presenter = AGamePresenter(view);

        presenter.Present();

        view.Received(Once).InitializeSpawners();
    }

    [Test]
    public void StartTimerWithSecondsWhenShowing()
    {
        var view = AView;
        var withSeconds = 10;
        var presenter = AGamePresenter(view, withSeconds);

        presenter.OnShow();

        view.Received(Once).StartTimer(withSeconds);
    }

    [Test]
    public void Sarasa()
    {
        var view = AView;
        var presenter = AGamePresenter(view);

        presenter.Present();

        view.Received(Once).InitializeSpawners();
    }

    private static IGameView AView
    {
        get
        {
            var mock = Substitute.For<IGameView>();
            return mock;
        }
    }

    private static GamePresenter AGamePresenter(IGameView withView = null, float withSeconds = 100)
    {
        return new GamePresenter(withView ?? AView, withSeconds);
    }
}
