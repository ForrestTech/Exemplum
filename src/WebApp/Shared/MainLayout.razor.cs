namespace Exemplum.WebApp.Shared
{
    using Extensions;
    using Microsoft.AspNetCore.Components;
    using MudBlazor;

    public partial class MainLayout : LayoutComponentBase
    {
        bool drawerOpen = false;
        MudTheme currentTheme = new();
        readonly MudTheme defaultTheme = new() { Palette = new Palette() { Black = "#272c34" } };

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        
        protected override void OnInitialized()
        {
            currentTheme = defaultTheme;
            //if not home page, the navbar starts open
            if (!NavigationManager.IsHomePage())
            {
                drawerOpen = true;
            }
        }

        void DrawerToggle()
        {
            drawerOpen = !drawerOpen;
        }

        void DarkMode()
        {
            currentTheme = currentTheme == defaultTheme ? darkTheme : defaultTheme;
        }
        
        void OnSwipe(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.LeftToRight when !drawerOpen:
                    drawerOpen = true;
                    StateHasChanged();
                    break;
                case SwipeDirection.RightToLeft when drawerOpen:
                    drawerOpen = false;
                    StateHasChanged();
                    break;
            }
        }
        
        readonly MudTheme darkTheme =
            new()
            {
                Palette = new Palette()
                {
                    Primary = "#776be7",
                    Black = "#27272f",
                    Background = "#32333d",
                    BackgroundGrey = "#27272f",
                    Surface = "#373740",
                    DrawerBackground = "#27272f",
                    DrawerText = "rgba(255,255,255, 0.50)",
                    DrawerIcon = "rgba(255,255,255, 0.50)",
                    AppbarBackground = "#27272f",
                    AppbarText = "rgba(255,255,255, 0.70)",
                    TextPrimary = "rgba(255,255,255, 0.70)",
                    TextSecondary = "rgba(255,255,255, 0.50)",
                    ActionDefault = "#adadb1",
                    ActionDisabled = "rgba(255,255,255, 0.26)",
                    ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                    Divider = "rgba(255,255,255, 0.12)",
                    DividerLight = "rgba(255,255,255, 0.06)",
                    TableLines = "rgba(255,255,255, 0.12)",
                    LinesDefault = "rgba(255,255,255, 0.12)",
                    LinesInputs = "rgba(255,255,255, 0.3)",
                    TextDisabled = "rgba(255,255,255, 0.2)",
                    Info = "#3299ff",
                    Success = "#0bba83",
                    Warning = "#ffa800",
                    Error = "#f64e62",
                    Dark = "#27272f"
                }
            };
    }
}