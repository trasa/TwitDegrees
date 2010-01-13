using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using TwitDegrees.GraphViewer.Infrastructure;
using TwitDegrees.GraphViewer.Infrastructure.ServiceClient;

namespace TwitDegrees.GraphViewer.Modules.Graph
{
    public class GraphViewModel : ViewModelBase
    {
        private readonly ITwitterClient twitterClient;


        public GraphViewModel()
        {
            // for blend
        }

        [InjectionConstructor]
        public GraphViewModel(ITwitterClient twitterClient)
        {
            this.twitterClient = twitterClient;
        }

        private int maxTreeDepth = 7;
        public int MaxTreeDepth { get { return maxTreeDepth;}  set { maxTreeDepth = value; InvokePropertyChanged("MaxTreeDepth");} }

        private int spacing = 75;
        public int Spacing { get { return spacing; } set { spacing = value; InvokePropertyChanged("Spacing");} }

        private string infoText = "Ok ready.";
        public string InfoText { get { return infoText; } set { infoText = value; InvokePropertyChanged("InfoText");} }

        private string selectedUserName = "trasa";
        public string SelectedUserName { get{ return selectedUserName;} set { selectedUserName = value; InvokePropertyChanged("SelectedUserName");} }
        


        public ICommand ResetCommand { get; private set; }
        public ICommand CanvasSelectedCommand { get; private set; }
        public ICommand FadeOutCompleteCommand { get; private set; }

        public IGraphView View { get; set; }

        protected override void WireUpEvents()
        {
            base.WireUpEvents();
            ResetCommand = new DelegateCommand<object>(o => Reset());
            CanvasSelectedCommand = new DelegateCommand<object>(CanvasSelected);
            FadeOutCompleteCommand = new DelegateCommand<object>(FadeOutComplete);
            
        }


        private void FadeOutComplete(object obj)
        {
            twitterClient.GetUser(SelectedUserName, result =>
                                               {
                                                   View.LoadModel(result);
                                                   View.RunFadeInStoryboard();
                                               });
        }


        private void CanvasSelected(object obj)
        {
            //throw new NotImplementedException();
        }


        private void Reset()
        {
            View.RunFadeOutStoryboard();
        }
    }
}
