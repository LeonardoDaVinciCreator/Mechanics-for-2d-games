## 📖 Contents / Содержание
| # | English | Русский |
|---|---------|---------|
| 1 | [Slingshot Mechanics](#slingshot-mechanics-angry-birds-style) | [Механика рогатки](#механика-рогатки-стиль-angry-birds) |
| 2 | [Wall Climber Mechanics](#wall-climber-mechanics) | [Механика Wall Climber](#механика-wall-climber) 
| 3 | [Swipe Detection System](#swipe-detection-system) | [Система определения свайпов](#система-определения-свайпов) 
| 4 | [UI Toolkit Integration](#ui-toolkit-integration) | [Интеграция UI Toolkit](#интеграция-ui-toolkit) 

## Mechanics-for-2d-games
###  **Slingshot Mechanics** *(Angry Birds Style)*

#### **Core Features**
- Drag-to-launch system with min/max distance limits
- Real-time physics-based trajectory prediction using LineRenderer
- Separate controllers for different object types (Birds, Climbers)
- Input System integration
- Dynamic velocity calculation based on angle and distance

###  **Wall Climber Mechanics**

#### **Core Features**
- Direct object throwing without slingshot mechanics
- Wall attachment via physics joints after collision
- No trajectory prediction (real-time object control)
- Input System integration for throw and climb controls
- Dynamic movement between multiple wall surfaces

#### **Wall Type**
- RegularWall - standard wall for climbing
- MovingWall - moving platforms, climber moves with the wall
- DisappearingWall - fade in/out walls (DOTween integration)
- BouncingWall - bounce wall 
- SlidingWall - planned feature for slippery surfaces

### **Swipe Detection System**

#### **Core Features**
- **Cross-platform**: Mouse + Touchscreen (PC/Mobile) + Arrow keys + Gamepad support (to be added)
- Single swipes with minimum distance threshold (`_swipeDistance`)
- **Click/Swipe differentiation**: If `delta.magnitude < _swipeDistance` — treated as click (object stops)
- **4 directions**: Left/Right/Up/Down with automatic dominant axis selection

## **UI Toolkit Integration**

Added **UI Toolkit** for optimized application performance by eliminating GameObject creation for UI elements. Interface similar to **WPF**.

**WPF Analogy**:
- **UXML** = XAML (interface markup)
- **USS** = CSS (styling) — styles work like regular CSS
- **C# code** = JS

**Setup**:
1. Create **UI Document** component on GameObject
2. Assign **UXML file** to Source field
3. Attach script for element interaction

**Element Search (UQuery)**:
- `Q<T>()` — first element in sequence
- `Query<T>()` — all elements in sequence

**Search Examples**:
```
// First element with class "panel"
_panel = _root.Q<VisualElement>("panel");

// All buttons with class "menu-button" in panel
List<Button> buttons = _panel.Query<Button>(className: "menu-button").ToList();
```
Data Operations:

Label.text for text changes (similar to TMPro)

Button.clicked += handler for events

## Механики для 2D игр
### **Механика рогатки** *(стиль Angry Birds)*

#### **Основные возможности**
- Система натяжения с ограничениями минимальной/максимальной дистанции
- Прогнозирование траектории в реальном времени на базе физики (LineRenderer)
- Отдельные контроллеры для разных типов объектов (Birds, Climbers)
- Интеграция Input System
- Динамический расчёт скорости по углу и дистанции

### **Механика Wall Climber**

#### **Основные возможности**
- Прямой бросок объекта без рогатки
- Прилипание к стене через физические соединения при столкновении
- Отсутствие прогнозирования траектории (управление в реальном времени)
- Интеграция Input System для броска и перемещения по стенам
- Динамическое переключение между несколькими поверхностями стен

#### **Типы стен**:
- RegularWall - стандартная стена для лазания
- MovingWall - движущиеся платформы, climber движется вместе со стеной
- DisappearingWall - стены с эффектом появления/исчезновения через изменения прозрачности (интеграция DOTween)
- BouncingWall - пока базово: реализация через Physic material 2d (пока отскок срабатывает назад на позицию, вместо отскока зеркально)
- SlidingWall - пока не начата работа, скользкие поверхности(поднятие вверх и вниз)



### **Система определения свайпов**

#### **Основные возможности**
- Кроссплатформенность: Мышь + тачскрин (ПК/мобильные) + добавить для стрелок и для геймпада
- Одинарные свайпы с минимальной дистанцией (```_swipeDistance```)
- Различение клика/свайпа: Если ```delta.magnitude < _swipeDistance``` — считается кликом (объект не двигается)
- 4 направления: Лево/Право/Вверх/Вниз с автоматическим выбором по доминирующей оси

## **Интеграция UI Toolkit**

Добавлен UI Toolkit для более оптимизированной работы приложения за счёт отсутствия создания GameObject'ов для UI элементов. Интерфейс похож на WPF.
Аналогия с WPF:
- UXML = XAML (разметка интерфейса)
- USS = CSS (стилизация) — стили работают аналогично CSS
- C# код = JS

Настройка
1. Создать UI Document компонент на GameObject
2. Назначить UXML файл в поле Source
3. Подключить скрипт для взаимодействия с элементами

Поиск элементов:
- ```Q<T>()``` - первый элемент в последовательности
- ```Query<T>()``` - все элементы в последовательности

Примеры поиска:
```
// Первый элемент с классом "panel"
_panel = _root.Q<VisualElement>("panel");

// Все кнопки с классом "menu-button" в панели
List<Button> buttons = _panel.Query<Button>(className: "menu-button").ToList();
```
Работа с данными:
- Label.text для изменения текста (аналогично TMPro)
- Button.clicked += handler — события

## Текущая версия юнити: 6000.3.10f1
