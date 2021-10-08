using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleScreenNavigator
{
    private readonly List<IHideable> gameViews;
    private IHideable currentScreen;
    private IHideable currentPopUp;

    public SimpleScreenNavigator(List<IHideable> gameViews)
    {
        this.gameViews = gameViews;
    }

    public virtual void InitializeViews()
    {
        gameViews.ForEach(view => view.Initialize());
    }

    //Método para mostrar una pantalla
    public virtual T ShowScreen<T>() where T : IScreen
    {
        return ShowScreen<T>(screen => screen.Show());
    }

    //Método para mostrar un popup
    public virtual T ShowPopUp<T>() where T : IPopUp
    {
        return ShowPopUp<T>(popup => popup.Show());
    }

    //Abro una pantalla, cierro el anterior
    public virtual T ShowScreen<T>(Action<T> showAction) where T : IScreen
    {
        currentScreen?.Hide();
        currentPopUp?.Hide();
        currentPopUp = null;

        var newScreen = FindViewOfType<T>();
        currentScreen = newScreen;
        showAction(newScreen);
        return newScreen;
    }

    //Abro un popup, cierro lo anterior
    public virtual T ShowPopUp<T>(Action<T> showAction) where T : IPopUp
    {
        currentPopUp?.Hide();

        var newPopup = FindViewOfType<T>();
        currentPopUp = newPopup;
        showAction(newPopup);
        return newPopup;
    }

    //Busco el tipo de pantalla o popup a abrir en la lista de pantallas
    private T FindViewOfType<T>() where T : IHideable
    {
        var found = (T)gameViews.FirstOrDefault(view => view is T);

        if (found == null)
        {
            throw new Exception($"Can't find a reference for screen of type {typeof(T)}");
        }

        return found;
    }
}

public interface IScreen : IHideable
{
    void Show();
}

public interface IPopUp : IHideable
{
    void Show();
}

public interface IHideable
{
    void Initialize();
    void Hide();
}