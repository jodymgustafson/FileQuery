using System;
using System.ComponentModel;
using System.Windows.Threading;
using FileQuery.Core;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Util
{
    public class QueryProcessManagerArgs : EventArgs
    {
        public Exception Error { get; set; }
    }

    /// <summary>
    /// Manages the background thread that the search query runs on.
    /// Tightly coupled to SearchControl.xaml.cs.
    /// </summary>
    class QueryProcessManager
    {
        private BackgroundWorker backgroundWorker;
        private SearchControlViewModel viewModel;
        private FileQueryProcessor queryProc;
        private Dispatcher dispatcher { get; }

        public event EventHandler<QueryProcessManagerArgs> SearchCompleted;

        public bool IsRunning { get { return backgroundWorker.IsBusy; } }

        public Exception LastError { get; private set; }

        public QueryProcessManager(SearchControlViewModel vm, Dispatcher dispatcher)
        {
            viewModel = vm;
            this.dispatcher = dispatcher;

            backgroundWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        public void Start(Query query)
        {
            backgroundWorker.RunWorkerAsync(query);
        }

        public void Abort()
        {
            queryProc.Cancel();
            backgroundWorker.CancelAsync();
        }

        private void Dispatch(Action action)
        {
            dispatcher.BeginInvoke(action);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatch(new Action(() => viewModel.IsSearching = false));
            Dispatch(new Action(() => SearchCompleted?.Invoke(this, new QueryProcessManagerArgs { Error = LastError })));
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatch(new Action(() => viewModel.IsSearching = true));
            RunFileQuery(e.Argument as Query);
        }

        private void RunFileQuery(Query query)
        {
            if (queryProc == null)
            {
                queryProc = new FileQueryProcessor();
                queryProc.FileFound += QueryProc_FileFound;
            }

            try
            {
                LastError = null;
                queryProc.Execute(query);
            }
            catch (Exception ex)
            {
                LastError = ex;
            }
        }

        private void QueryProc_FileFound(object sender, FileFoundEventArgs e)
        {
            Dispatch(new Action(() => viewModel.SearchResults.Add(e.fileInfo.FullName)));
        }
    }
}
