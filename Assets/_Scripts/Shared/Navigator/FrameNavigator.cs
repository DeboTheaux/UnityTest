using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UT.Shared
{
    public class FrameNavigator
    {
        private readonly List<FramePath> _framePaths;

        private delegate IFrame SearchFrame(IFrame frame);
        private event SearchFrame searchFrame = _ => _;

        IFrame _currentScreen;
        IFrame _currentPopup;

        public FrameNavigator(List<FramePath> framePaths)
        {
            _framePaths = framePaths;
        }

        public void InitializeFrames()
        {
            _framePaths.ForEach(path => path.Frame.GetComponent<IFrame>().Initialize());
        }

        public void OpenFrameById(string id)
        {
            var frameFounded = searchFrame(FindFrameWithId(id));
            OpenFrame(frameFounded);
        }

        public T OpenFrameByType<T>() where T : IFrame
        {
            var frameFounded = searchFrame(FindFrameOfType<T>());
            return (T)OpenFrame(frameFounded);
        }

        #region OpenFrame

        private IFrame OpenFrame(IFrame frameToOpen)
        {
            if (frameToOpen is IPopUp)
            {
                return OpenPopUp((IPopUp)frameToOpen);

            }
            else if (frameToOpen is IScreen)
            {
                return OpenScreen((IScreen)frameToOpen);
            }
            else
            {
                throw new Exception($"Frame {frameToOpen} does'nt inherit from IPopUp or IScreen");
            }
        }

        private IPopUp OpenPopUp(IPopUp popUpToOpen)
        {
            _currentPopup?.Hide();
            _currentPopup = popUpToOpen;

            popUpToOpen.Show();

            return popUpToOpen;
        }

        private IScreen OpenScreen(IScreen screenToOpen)
        {
            _currentPopup?.Hide();
            _currentPopup = null;
            _currentScreen?.Hide();
            _currentScreen = screenToOpen;
            _currentScreen.Show();

            return screenToOpen;
        }

        # endregion

        #region SearchFrame

        private T FindFrameOfType<T>() where T : IFrame =>
            (T)FindFrameOfType(typeof(T));

        private IFrame FindFrameOfType(Type frameType)
        {
            var foundByTypeInterface = SearchFrameOfTypeInterface(_framePaths.Select(path => path.Frame.GetComponent<IFrame>()), frameType);
            var foundByTypeClass = SearchFrameOfTypeClass(_framePaths.Select(path => path.Frame.GetComponent<IFrame>()), frameType);
            if (foundByTypeClass == null && foundByTypeInterface == null)
                throw new Exception($"Can't find a reference for screen of type {frameType}");
            return foundByTypeInterface ?? foundByTypeClass;
        }

        private IFrame FindFrameWithId(string id)
        {
            var foundById = SearchFrameWithId(_framePaths, id);
            if (foundById == null)
                throw new Exception($"Can't find a reference for screen with id {id}");
            return foundById;
        }

        private IFrame SearchFrameOfTypeInterface(IEnumerable<IFrame> frames, Type frameType) =>
            frames.FirstOrDefault(frame => frame.GetType().GetInterface(frameType.ToString()) == frameType);
        private IFrame SearchFrameOfTypeClass(IEnumerable<IFrame> frames, Type frameType) =>
            frames.FirstOrDefault(frame => frame.GetType() == frameType);
        private IFrame SearchFrameWithId(IEnumerable<FramePath> framePaths, string id) =>
            framePaths.FirstOrDefault(path => path.Id == id).Frame.GetComponent<IFrame>();

        #endregion

    }

    public interface IFrame
    {
        void Initialize();
        void Show();
        void Hide();
    }

    public interface IScreen : IFrame
    {

    }

    public interface IPopUp : IFrame
    {

    }

    [Serializable]
    public struct FramePath
    {
        public string Id;
        public GameObject Frame;
    }
}