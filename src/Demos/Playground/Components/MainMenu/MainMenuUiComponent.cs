using System;
using lunge.Library.Debugging.Logging;
using Nez;
using Nez.UI;
using Playground.Scenes;

namespace Playground.Components.MainMenu;

public class MainMenuUiComponent : Component
{
    enum MenuType
    {
        MainMenu,
        Options,
        StartGame
    }

    private UICanvas _canvas;
    private Skin _skin;
    private MenuType _currentType = MenuType.MainMenu;
    private Table _table;

    private readonly ILogger _logger = LoggerFactory.GetLogger<MainMenuUiComponent>();

    public MainMenuUiComponent()
    {
        _skin = Core.Services.GetService<Skin>();
    }

    public override void OnAddedToEntity()
    {
        _canvas = Entity.GetComponent<UICanvas>();
        SetupUi();
    }

    public override void DebugRender(Batcher batcher)
    {
        base.DebugRender(batcher);
        _table?.DebugRender(batcher);
    }

    private void SetupUi()
    {
        _table = _canvas.Stage.AddElement(new Table());
        _table.DebugAll();
        _table.SetDebug(true);

        SwitchTo(MenuType.MainMenu);
    }

    private void SwitchTo(MenuType menuType)
    {
        _currentType = menuType;
        _table.Clear();

        switch (_currentType)
        {
            case MenuType.MainMenu:
                CreateMainMenu();
                break;
            case MenuType.Options:
                CreateOptions();
                break;
            case MenuType.StartGame:
                CreateStartGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(menuType), menuType, null);
        }
    }

    private void CreateMainMenu()
    {
        // var menus = new Table().PadLeft(50);

        _table.SetFillParent(true).Center();
        
        _table.Add(new TextButton("Start Game", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
            {
                _logger.Debug("start game clicked");
                SwitchTo(MenuType.StartGame);
            };

        _table.Row().SetPadTop(10);

        _table.Add(new TextButton("Options", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
            {
                _logger.Debug("options clicked");
                SwitchTo(MenuType.Options);
            };

        _table.Row().SetPadTop(10);

        _table.Add(new TextButton("Exit", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
            {
                _logger.Debug("exit clicked");
                Core.Exit();
            };
    }

    private void CreateOptions()
    {
        _table.SetFillParent(true).Center();

        _table.Row().SetPadTop(10);

        _table.Add(new ProgressBar(0, 100, 1, false, _skin)).SetMinHeight(16).SetMinWidth(250)
            .GetElement<ProgressBar>().Value = 23;

        _table.Row().SetPadTop(10);

        var cbBorderless = _table.Add(new CheckBox("Borderless", _skin)).Left()
            .GetElement<CheckBox>();

        cbBorderless.IsChecked = Core.Instance.Window.IsBorderlessEXT;
        cbBorderless.OnChanged += b =>
        {
            Core.Instance.Window.IsBorderlessEXT = b;
        };

        _table.Row().SetPadTop(10);

        _table.Add(new TextButton("Back", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
        {
            _logger.Debug("back clicked");
            SwitchTo(MenuType.MainMenu);
        };

        // _table.Add(new NumberField(12, 0, 100, 1, true, new NumberFieldStyle()));
    }

    private void CreateStartGame()
    {
        _table.Add(new TextButton("Single Player", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
            {
                Core.StartSceneTransition(new FadeTransition(() => new SimpleGameplayScene()));
            };

        _table.Row().SetPadTop(10);

        _table.Add(new TextButton("Back", _skin)).SetFillX().SetMinHeight(30).SetMinWidth(250)
            .GetElement<Button>().OnClicked += button =>
            {
                _logger.Debug("back clicked");
                SwitchTo(MenuType.MainMenu);
            };
    }
}