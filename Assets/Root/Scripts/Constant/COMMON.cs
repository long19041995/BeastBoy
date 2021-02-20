using System.Collections;
using System.Collections.Generic;

namespace Const
{
    public class Common
    {
        public enum WAVE_STATE { PASS, FAIL }

        public enum POPUP { COMPLETE, GAME_OVER, CONTINUE }

        public enum LAYER { DEFAULT, BACKGROUND, ITEM, CHARACTER, WALL, LAND, ANSWER, PROGRESS_BAR, POPUP }
        public static List<string> listLayer = new List<string>{ "Default", "Background", "Item", "Character", "Wall", "Land", "Answer", "ProgressBar", "Popup" };

        public enum AUDIOS { AIRPLANE, BREATHING, DINO1, DINO2, DOOR, DRAGON, EARTHQUAKE, ELECTRIC, FAIL_CHOOSE1, FAIL_CHOOSE2, FLY, GAME_OVER, GORILLA, IN_GAME_BACKGROUND, KARATE, LASER, MOUNTAIN, PASS, ROBOT, RUN1, RUN2, SCREAM, TOUCH_UI1, TRANSFIGURE, UI_BACKGROUND, WALK, WALL_FALL, WHALE, WIND, WIN_POPUP, WIN_POPUP_ADDED }

        public static List<string> audios = new List<string>{ "Airplane", "Breathing", "Dino1", "Dino2", "Door", "Dragon", "Earthquake", "Electric", "FailChoose1", "FailChoose2", "Fly", "GameOver", "Gorilla", "InGameBackground", "Karate", "Laser", "Mountain", "Pass", "Robot", "Run1", "Run2", "Scream", "TouchUI1", "Transfigure", "UiBackground", "Walk", "WallFall", "Whale", "WinPopup", "WinPopupAdded" };

        public const string INDEX_MAP = "INDEX_MAP";
        public const string INDEX_LEVEL = "INDEX_LEVEL";
        public const string INDEX_WAVE = "INDEX_WAVE";
        public const string INDEX_PASSED = "INDEX_PASSED";
        public const string COIN = "COIN";
        public const string INDEX_INGAME = "INDEX_INGAME";
        public const string IS_MUTE_SOUND = "IS_MUTE_SOUND";
        public const string IS_MUTE_SOUND_ALL = "IS_MUTE_SOUND_ALL";
    }
}
