using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DynamicDataGridTest.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		[Reactive]
		public SupplierCodeViewModel? CurrentSupplier { get; set; }

		public ObservableCollectionExtended<SupplierCodeViewModel> Results { get; } = new();

		readonly SourceCache<SupplierCodeViewModel, string> supplierCodes = new(k => k.Supplier);

		public ReactiveCommand<AddingNewItemEventArgs, Unit> AddingNewItem { get; private set; } = null!;

		public ReactiveCommand<DataGridBeginningEditEventArgs, Unit> BeginningEdit { get; private set; } = null!;

		public ReactiveCommand<DataGridCellEditEndingEventArgs, Unit> CellEditEnding { get; private set; } = null!;

		public ReactiveCommand<InitializingNewItemEventArgs, Unit> InitializingNewItem { get; private set; } = null!;

		public ReactiveCommand<DataGridPreparingCellForEditEventArgs, Unit> PreparingCellForEdit { get; private set; } = null!;

		public ReactiveCommand<DataGridRowEditEndingEventArgs, Unit> RowEditEnding { get; private set; } = null!;

		public ReactiveCommand<SelectionChangedEventArgs, Unit> SelectionChanged { get; private set; } = null!;

		public ReactiveCommand<Unit, IEnumerable<SupplierCodeViewModel>> Reload { get; private set; } = null!;

		public MainWindowViewModel()
		{
			_ = this.supplierCodes.Connect().AutoRefresh().Do(x =>
			{
				Debug.WriteLine("AutoRefresh:");
				foreach (var y in x)
					Debug.WriteLine($"             {y}");
			}).ObserveOn(RxApp.MainThreadScheduler).Do(x =>
			{
				Debug.WriteLine("ObserveOn:");
				foreach (var y in x)
					Debug.WriteLine($"           {y}");
			}).Bind(this.Results).Do(x =>
			{
				Debug.WriteLine("Bind:");
				foreach (var y in x)
					Debug.WriteLine($"      {y}");
			}).Subscribe();

			this.AddingNewItem = ReactiveCommand.Create<AddingNewItemEventArgs>(e => Debug.WriteLine($"AddingNewItem - e.NewItem: {e.NewItem}"));
			this.BeginningEdit = ReactiveCommand.Create<DataGridBeginningEditEventArgs>(e => Debug.WriteLine($"BeginningEdit - e.Cancel: {e.Cancel} | e.Column: {e.Column} | e.Row: {e.Row}"));
			this.CellEditEnding = ReactiveCommand.Create<DataGridCellEditEndingEventArgs>(e => Debug.WriteLine($"CellEditEnding - e.Cancel: {e.Cancel} | e.Column: {e.Column} | e.EditAction: {e.EditAction} | e.EditingElement: {e.EditingElement} | e.Row: {e.Row}"));
			this.InitializingNewItem = ReactiveCommand.Create<InitializingNewItemEventArgs>(e => Debug.WriteLine($"InitializingNewItem - e.NewItem {e.NewItem}"));
			this.PreparingCellForEdit = ReactiveCommand.Create<DataGridPreparingCellForEditEventArgs>(e => Debug.WriteLine($"PreparingCellForEdit - e.Column: {e.Column} | e.EditingElement: {e.EditingElement} | e.Row: {e.Row}"));
			this.RowEditEnding = ReactiveCommand.Create<DataGridRowEditEndingEventArgs>(e => Debug.WriteLine($"RowEditEnding - e.Cancel: {e.Cancel} | e.EditAction: {e.EditAction} | e.Row: {e.Row}"));
			this.SelectionChanged = ReactiveCommand.Create<SelectionChangedEventArgs>(e => Debug.WriteLine($"SelectionChanged - e.AddedItems: {e.AddedItems} | e.Handled: {e.Handled} | e.RemovedItems: {e.RemovedItems} | e.Source: {e.Source}"));

			this.Reload = ReactiveCommand.Create(MainWindowViewModel.GetSuppliersImpl);

			_ = this.Reload.Subscribe(codes => this.supplierCodes.Edit(innerList =>
			{
				innerList.Clear();
				innerList.AddOrUpdate(codes);
			}));
		}

		static IEnumerable<SupplierCodeViewModel> GetSuppliersImpl()
		{
			yield return new("TEST1", "TEST1CODE");
			yield return new("TEST2", "TEST2CODE");
		}
	}
}
