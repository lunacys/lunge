
(function createTable() {
    const table = new Table();

    table.Left().Top().PadTop(4);
    table.SetFillParent(true);
    table.Defaults().SetPadTop(4).SetPadRight(4).SetPadLeft(4).SetAlign(Align.Left);

    const drawable: IDrawable = new PrimitiveDrawable(new Color(40, 40, 40, 220));
    table.SetBackground(drawable);

    const scrollPaneRight = UiCanvas.Stage.AddElement(new ScrollPane(table, Skin));
    scrollPaneRight.SetPosition(Screen.Width - 340, 0);
    // force a validate which will layout the ScrollPane and populate the proper scrollBarWidth
    scrollPaneRight.Validate();
    scrollPaneRight.SetSize(340 + scrollPaneRight.GetScrollBarWidth(), Screen.Height);

    const t: string = typeof(table);

    return table;
})();