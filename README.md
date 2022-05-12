# DevSpectorDesktop

## Инструкция
Приложение по умолчанию подключается к тестовому серверу, поэтому все изменения сохраняются до достижения получаса бездействия.
После этого срока все данные будут возвращены в изначальное состояние.

Чтобы подключиться к другому серверу, необходимо установить переменную среды `DEVSPECTOR_ENV` в значение `Production`, 
после чего записать название нужного хоста в переменную `DEVSPECTOR_HOST`. Порт по умолчанию имеет значение `80`, но это значение
можно изменить, дав новое значение переменной `DEVSPECTOR_PORT`.

Если программа неожиданно завершила свою работу, то информацию об ошибке можно найти в корне программы, в папке `logs`.
