using ReciptShare.Services;

namespace ReciptShare;

public partial class App : Application
{
    private readonly IAuthenticationService _authService;
    private readonly AppShell _shell;

    public App(AppShell shell, IAuthenticationService authService)
    {
        InitializeComponent();
        _shell = shell;
        _authService = authService;

        MainPage = _shell;

        _shell.Dispatcher.Dispatch(async () =>
        {
            if (!_authService.IsAuthenticated)
            {
                await _shell.GoToAsync("//login");
            }
        });
    }
}