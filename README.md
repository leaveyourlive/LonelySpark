# LonelySpark

![image](https://github.com/user-attachments/assets/3d9e543f-c0e1-4ceb-8506-9b1caa57c9f6)

Пользовательская документация:<br />
•	Платформа: Windows, разработка в Unity Editor 2022+<br />
•	Запуск: через Unity Editor (сцена MainScene) или экспорт в .exe<br />


Требования:<br />
o	ОС: Windows 10 и выше<br />
o	ОЗУ: от 4 ГБ<br />
o	Видео: поддержка DirectX 11<br />


Управление:<br />
o	WASD — перемещение персонажа<br />
o	ЛКМ / ПКМ — лёгкая и тяжёлая атаки<br />
o	E — взаимодействие (сбор предметов, NPC)<br />
o	комбинации клавишь Z, X, C, V + R — активация способностей<br />
o	Esc — пауза/меню<br />


Интерфейс отображает основные параметры:<br />
•	уровень здоровья<br />
•	количество монет<br />
•	активные способности<br />
•	подсказки для взаимодействия<br />


Техническая документация<br />


Блок-схема логики:<br />
1.Запуск сцены → инициализация игрока (позиция, параметры)<br />
2.Пользователь вводит команды: движение, атаки, активация способностей<br />
3.Атака игрока(врага) рядом с врагом(игроком): -hp у врага(игрока)<br />
4.Смерть врага → уничтожение объекта + добавление монет игроку<br />
5.Смерть игрока → сброс позиции и здоровья (с сохранением монет и умений)<br />
6.При накоплении нужной суммы → телепорт → конец уровня<br />


Взаимодействие объектов:<br />
•	Игрок: Rigidbody2D, скрипты PlayerStats, PlayerAttack, PlayerStateMachine<br />
•	Враги: Rigidbody2D, EnemyHealth, EnemyAttack, EnemyMeleeAttack<br />
•	NPC: Collider2D, скрипт NPCInteraction<br />
•	Интерфейс: Canvas с компонентами AbilityInformationUI и отображением статов<br />
•	Монеты и мешки: взаимодействие через PlayerInteraction.cs<br />


Файлы проекта (по папкам):<br />
•	Assets/Scenes — сцены: StartMenu, LoadScreen, MainLocation, ComingSoonScene. <br />
•	Assets/Scripts:
o	Player: PlayerStats.cs, PlayerAttack.cs, PlayerStateMachine.cs<br />
o	Enemy: EnemyHealth.cs, EnemyAttack.cs, EnemyMeleeAttack.cs<br />
o	Abilities: AbilityInputManager.cs, AbilityInformationUI.cs<br />
o	Interaction: NPCInteraction.cs, PlayerInteraction.cs<br />
•	Assets/Prefabs — игрок, враги, мешки, NPC<br />
•	Assets/Animations — анимации передвижения, атак и смерти<br />
•	Assets/UI — канвасы интерфейса, описание способностей, здоровье и монеты<br />


Название метода - Возвращаемый тип - Пояснение<br />
TakeDamage(int amount) - void - Уменьшает здоровье объекта (используется у врагов и игрока).<br />
Die() - void - Удаляет объект (врага) или сбрасывает здоровье (у игрока).<br />
Attack() - void - Атака игрока — проверка врагов в зоне удара и нанесение урона.<br />
ChangeState(State newState) - void - Смена состояния игрока (например, Idle → Attack).<br />
UseAbility(int index) - void - Активация способности, если она доступна и соответствует индексу.<br />
ShowInfo(string, string) - void - Отображение описания способности на UI.<br />
HideInfo() - void - Скрытие информации о способности.<br />
Interact() - void - Обработка взаимодействия с NPC или предметом.<br />
Collect() - void - Добавление монет игроку при сборе объекта.<br />
LearnAbility(string id) - void - Передаёт игроку способность от NPC.<br />
AttackPlayer() - void - Наносит урон игроку, если он находится в зоне атаки врага.<br />
AddCoins(int amount) - void - Увеличивает число собранных монет у игрока.<br />
CanUseAbility(int id) - bool - Возвращает true, если способность можно использовать.<br />
DisplayAbilityTooltip(string) - void - Отображает описание способности в интерфейсе.<br />
EnableInteractionPrompt() - void - Включает подсказку о доступном взаимодействии.<br />
DisableInteractionPrompt() - void - Выключает подсказку при выходе из зоны взаимодействия.<br />
