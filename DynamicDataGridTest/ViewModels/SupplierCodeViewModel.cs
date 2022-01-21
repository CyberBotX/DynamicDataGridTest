using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DynamicDataGridTest.ViewModels
{
	public class SupplierCodeViewModel : ReactiveObject
	{
		[Reactive]
		public string PartNumber { get; set; } = null!;

		[Reactive]
		public string Supplier { get; set; } = null!;

		public string OldSupplier { get; set; } = null!;

		[Reactive]
		public string Code { get; set; } = null!;

		public string OldCode { get; set; } = null!;

		public SupplierCodeViewModel()
		{
		}

		public SupplierCodeViewModel(string supplier, string code) : this()
		{
			this.Supplier = this.OldSupplier = supplier;
			this.Code = this.OldCode = code;
		}

		public override string ToString() => $"PartNumber = {this.PartNumber} | Supplier = {this.Supplier} (Old was {this.OldSupplier}) | Code = {this.Code} (Old was {this.OldCode})";
	}
}
