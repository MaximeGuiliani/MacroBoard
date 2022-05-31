using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.EventHandlers;
using System.Threading;

namespace FlaUInspect.Core
{
    public class FocusTrackingMode
    {
        private readonly AutomationBase _automation;
        private FocusChangedEventHandlerBase _eventHandler;
        private AutomationElement _currentFocusedElement;

        public event Action<AutomationElement> ElementFocused;
        

        public FocusTrackingMode(AutomationBase automation)
        {
            _automation = automation;
        }

        public void Start()
        {
            // Might give problems because inspect is registered as well.
            // MS recommends to call UIA commands on a thread outside of an UI thread.
            Task.Factory.StartNew(() => _eventHandler = _automation.RegisterFocusChangedEvent(OnFocusChanged));
        }

        public void Stop()
        {
            //_automation.UnregisterFocusChangedEvent(_eventHandler);
            //_eventHandler.Dispose();
            //_automation.Dispose();
            //_automation.UnregisterFocusChangedEvent(_eventHandler);
            Task.Factory.StartNew(() => _automation.UnregisterFocusChangedEvent(_eventHandler));
        }

        private void OnFocusChanged(AutomationElement automationElement)
        {
            // Skip items in the current process
            // Like Inspect itself or the overlay window
              //Trace.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>"+ automationElement.Properties.ProcessId.ToString());
              //Trace.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>" + Process.GetCurrentProcess().Id.ToString());

            if (automationElement.Properties.ProcessId == Process.GetCurrentProcess().Id)
            {
                return;
            }
            if (!Equals(_currentFocusedElement, automationElement))
            {
                _currentFocusedElement = automationElement;
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    ElementFocused?.Invoke(automationElement);
                });
            }
        }
    }
}
