# LonelySpark

Пользовательская документация
•	Платформа: Windows, разработка в Unity Editor 2022+
•	Запуск: через Unity Editor (сцена MainScene) или экспорт в .exe
•	Требования:
o	ОС: Windows 10 и выше
o	ОЗУ: от 4 ГБ
o	Видео: поддержка DirectX 11
•	Управление:
o	WASD — перемещение персонажа
o	ЛКМ / ПКМ — лёгкая и тяжёлая атаки
o	E — взаимодействие (сбор предметов, NPC)
o	1–4 + Enter — активация способностей
o	Esc — пауза/меню
Интерфейс отображает основные параметры:
•	уровень здоровья
•	количество монет
•	активные способности
•	подсказки для взаимодействия
5. Техническая документация
5.1 Блок-схема логики:
1.	Запуск сцены → инициализация игрока (позиция, параметры)
2.	Пользователь вводит команды: движение, атаки, активация способностей
3.	При столкновении с врагом — обмен уроном (PlayerAttack ⇄ EnemyHealth)
4.	Смерть врага → уничтожение объекта + добавление монет игроку
5.	Смерть игрока → сброс позиции и здоровья (с сохранением монет и умений)
6.	При накоплении нужной суммы → телепорт → конец уровня
5.2 Взаимодействие объектов:
•	Игрок: Rigidbody2D, скрипты PlayerStats, PlayerAttack, PlayerStateMachine
•	Враги: Rigidbody2D, EnemyHealth, EnemyAttack, EnemyMeleeAttack
•	NPC: Collider2D, скрипт NPCInteraction
•	Интерфейс: Canvas с компонентами AbilityInformationUI и отображением статов
•	Монеты и мешки: взаимодействие через PlayerInteraction.cs
5.3 Файлы проекта (по папкам):
•	Assets/Scenes — сцены: MainScene, Level1, Level2
•	Assets/Scripts:
o	Player: PlayerStats.cs, PlayerAttack.cs, PlayerStateMachine.cs
o	Enemy: EnemyHealth.cs, EnemyAttack.cs, EnemyMeleeAttack.cs
o	Abilities: AbilityInputManager.cs, AbilityInformationUI.cs
o	Interaction: NPCInteraction.cs, PlayerInteraction.cs
•	Assets/Prefabs — игрок, враги, мешки, NPC
•	Assets/Animations — анимации передвижения, атак и смерти
•	Assets/UI — канвасы интерфейса, описание способностей, здоровье и монеты


Название метода	Возвращаемый тип	Пояснение
TakeDamage(int amount)	void	Уменьшает здоровье объекта (используется у врагов и игрока).
Die()	void	Удаляет объект (врага) или сбрасывает здоровье (у игрока).
Attack()	void	Атака игрока — проверка врагов в зоне удара и нанесение урона.
ChangeState(State newState)	void	Смена состояния игрока (например, Idle → Attack).
UseAbility(int index)	void	Активация способности, если она доступна и соответствует индексу.
ShowInfo(string, string)	void	Отображение описания способности на UI.
HideInfo()	void	Скрытие информации о способности.
Interact()	void	Обработка взаимодействия с NPC или предметом.
Collect()	void	Добавление монет игроку при сборе объекта.
LearnAbility(string id)	void	Передаёт игроку способность от NPC.
Start()	void	Инициализация компонента на сцене.
Update()	void	Обработка ввода и поведения объекта.
OnTriggerEnter2D(Collider2D)	void	Вход в зону взаимодействия с объектом.
OnTriggerExit2D(Collider2D)	void	Выход из зоны взаимодействия.
AttackPlayer()	void	Наносит урон игроку, если он находится в зоне атаки врага.
AddCoins(int amount)	void	Увеличивает число собранных монет у игрока.
CanUseAbility(int id)	bool	Возвращает true, если способность можно использовать.
DisplayAbilityTooltip(string)	void	Отображает описание способности в интерфейсе.
EnableInteractionPrompt()	void	Включает подсказку о доступном взаимодействии.
DisableInteractionPrompt()	void	Выключает подсказку при выходе из зоны взаимодействия.
