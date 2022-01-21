using System.Reactive.Disposables;
using DynamicDataGridTest.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace DynamicDataGridTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			this.InitializeComponent();

			this.ViewModel = new MainWindowViewModel();

			_ = this.WhenActivated(disposableRegistration =>
			{
				_ = this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext).DisposeWith(disposableRegistration);

				_ = this.OneWayBind(this.ViewModel, vm => vm.Results, view => view.grid.ItemsSource).DisposeWith(disposableRegistration);
				_ = this.BindCommand(this.ViewModel, vm => vm.Reload, view => view.refresh).DisposeWith(disposableRegistration);

				_ = this.grid.Events().AddingNewItem.InvokeCommand(this.ViewModel, vm => vm.AddingNewItem).DisposeWith(disposableRegistration);
				_ = this.grid.Events().BeginningEdit.InvokeCommand(this.ViewModel, vm => vm.BeginningEdit).DisposeWith(disposableRegistration);
				_ = this.grid.Events().CellEditEnding.InvokeCommand(this.ViewModel, vm => vm.CellEditEnding).DisposeWith(disposableRegistration);
				_ = this.grid.Events().InitializingNewItem.InvokeCommand(this.ViewModel, vm => vm.InitializingNewItem).
					DisposeWith(disposableRegistration);
				_ = this.grid.Events().PreparingCellForEdit.InvokeCommand(this.ViewModel, vm => vm.PreparingCellForEdit).
					DisposeWith(disposableRegistration);
				_ = this.grid.Events().RowEditEnding.InvokeCommand(this.ViewModel, vm => vm.RowEditEnding).DisposeWith(disposableRegistration);
				_ = this.grid.Events().SelectionChanged.InvokeCommand(this.ViewModel, vm => vm.SelectionChanged).DisposeWith(disposableRegistration);
			});
		}
	}
}
