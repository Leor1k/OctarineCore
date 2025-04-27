using System;
using System.Linq;
using System.Windows.Input;
using GlobalHotKey;

namespace Octarine_Core.Classic
{
    public static class HotKeyParser
    {
        public static HotKey Parse(string hotkeyStr)
        {
            if (string.IsNullOrEmpty(hotkeyStr))
                return null;

            var parts = hotkeyStr.Split('+');

            if (!Enum.TryParse(parts.Last(), out Key key))
                throw new ArgumentException($"Некорректная клавиша: {parts.Last()}");

            var modifiers = ModifierKeys.None;
            foreach (var modStr in parts.Take(parts.Length - 1))
            {
                if (Enum.TryParse(modStr, out ModifierKeys mod))
                    modifiers |= mod;
                else
                    throw new ArgumentException($"Некорректный модификатор: {modStr}");
            }

            return new HotKey(key, modifiers);
        }

        public static string ToString(HotKey hotkey)
        {
            if (hotkey == null)
                return string.Empty;

            var modifiers = hotkey.Modifiers;
            var result = string.Empty;

            if (modifiers.HasFlag(ModifierKeys.Control))
                result += "Ctrl+";
            if (modifiers.HasFlag(ModifierKeys.Alt))
                result += "Alt+";
            if (modifiers.HasFlag(ModifierKeys.Shift))
                result += "Shift+";
            if (modifiers.HasFlag(ModifierKeys.Windows))
                result += "Win+";

            return result + hotkey.Key;
        }
    }
}
