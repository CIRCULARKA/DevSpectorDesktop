# DevSpectorDesktop

## Инструкция

Если программа неожиданно завершила свою работу или вовсе не запустилась, то причину ошибки можно
найти в корневом каталоге, в папке `logs`.

# Использование тестового сервера
Приложение по умолчанию подключается к тестовому серверу, поэтому все изменения сохраняются до достижения получаса бездействия.
После этого срока все данные будут возвращены в изначальное состояние.

# Подключение к другому серверу
Чтобы подключиться к другому серверу, необходимо установить переменную среды `DEVSPECTOR_ENV` в значение `Production`, 
после чего записать название нужного хоста в переменную `DEVSPECTOR_HOST`. Порт по умолчанию имеет значение `80`, но это значение
можно изменить, дав новое значение переменной `DEVSPECTOR_PORT`.
