# Unnamed Stress Testing

Программа, предоставляющая возможность выучить ударения без особой боли для ЕГЭ.

Написано на C#, используя __WPF__ и паттерн проектирования __MVVM__.

![Скриншот](https://raw.githubusercontent.com/R3dKar/UnnamedStressTesting/master/Screenshots/screenshot1.png)

## Установка

Windows:

* Скачать архив [релиза](https://github.com/R3dKar/UnnamedStressTesting/releases/tag/v1.0.0)
* Распаковать и запустить ``UnnamedStressTesting.exe``

## Использование

Выбираем слова и начинаем тест. Словари можно скачать с GitHub (в программе есть кнопка) или создать и отредактировать самостоятельно.

### Формат словарей

Словари лежат в ``%ИмяПользователя%\AppData\Local\Unnamed Stress Testing\Словари``

Файлы должны быть формата ``.txt``

```
слОво [+ включено или - выключено] [коментарий при наведении на солво]
```

Части в квадратных скобках необязательны. Слова записываются каждое в новой строчке.

## Лицензия

Распространяется под лицензией _GNU General Public License v3.0_ (смотри файл ``LICENSE``)

## Использованные библиотеки
* [FontAwesome5 WPF](https://github.com/MartinTopfstedt/FontAwesome5)
* [Fody PropertyChanged](https://github.com/Fody/PropertyChanged)
* [WPF Task Dialog Wrapper](https://github.com/yadyn/WPF-Task-Dialog)