# Spear
Точкой входа является класс - GameManager. Игра построена на конечном автомате, где - StartGame, GameProcess, GameOver, ... - являются игровыми состояниями.

Важное место занимают классы, начинающиеся на Proessing... Это независимые модули, которые выполняют строго свою определенную функцию. При отключении любого из них, приоложение не выдаст ошибок. Будет лишь убран функционал, который обеспечивает Processing.
