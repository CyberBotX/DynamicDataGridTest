# DynamicData Grid Test
A test showing what happens when using [DynamicData](https://github.com/reactivemarbles/DynamicData) with of a WPF `DataGrid`.

The `ItemsSource` of the DataGrid is being bound using [ReactiveUI](https://github.com/reactiveui/ReactiveUI).

Because of the complexity of the interaction between DynamicData's `SourceCache` and UI binding, changes from the UI side of things in most cases do not affect the `SourceCache`, leading to a strong disconnect between the two.

In this simple example, the view model contains a `SourceCache` that is bound to a DynamicData `ObservableCollectionExtended`. That collection is one-way bound in the view to the `ItemsSource` of a WPF `DataGrid`.

To propagate the `SourceCache`, there is a button in the UI which calls a `ReactiveCommand` that clears out the cache and fills in 2 values.

This example should be run within Visual Studio, as there are `Debug.WriteLine` calls to show when certain events are happening on the `DataGrid` itself.

Things to note:
- Adding a new row in the UI does not trigger any `SourceCache` notifications.
- Deleting an existing row, even one which came from the `SourceCache`, does not trigger any `SourceCache` notifications.
- Editing a row has one of two things happen:
  - If the row exists in the `SourceCache`, after the row has finished being edited, results in 2 `Refresh` notifications to happen on the item that was edited.
  - If the row does not exist in the `SourceCache`, there is no notification from the `SourceCache`.
- If a new row has been added to the UI and then the `Refresh` button is clicked, it will remove the rows contained in the `SourceCache` and add them back in, but will not remove the new row due to it not being in the `SourceCache`.

Although this example uses the `DataGrid` that comes with WPF, I have had similar issues when using `RadGridView` from [Telerik UI for WPF](https://www.telerik.com/products/wpf/overview.aspx). The primary difference is that when editing a row that exists in the `SourceCache`, the `Refresh` notifications happen after each cell edit has concluded instead of after the whole row.

While not shown in this example, if any of the UI events attempt to add a new item to the `SourceCache`, this can result in a much stronger disconnect between the `SourceCache` and the UI collection, as it can lead to there being a duplicate of the item shown in the UI that isn't removed if the `SourceCache` is repopulated. The reason for this example not actually adding a row is due to the limited functionality of the WPF `DataGrid`, namely that the event for adding a new row would use an object that contains a null string which causes an exception as the `SourceCache` cannot have a null key.
