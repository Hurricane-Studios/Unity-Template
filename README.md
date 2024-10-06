# Unity-Template 3D URP (Universal Render Pipeline)
## Using Ver 2022.3.48f1 
## Join Our Discord if you need help or want to follow our other projects! https://discord.gg/XgS5Cuus7s
most likely, we will add more versions later on but this is where we will start
Also, anyone can contribute to this, this is to help newer devs get into the motions and give them a little more ground to stand on
For anyone who needs this, I recommend a code editor Visual Studio Code (VSC) or Visual Studio (VS)

### Script Explanations with Code Snippets
PlayerStats.cs
Start(): Initializes the player's health and stamina to their maximum values and sets up references, including the Animator component.
```
void Start()
{
    health = maxHealth;
    stamina = maxStamina;
    animator = GetComponentInChildren<Animator>();
}
```
Update(): Rounds health and stamina to ensure they are whole numbers and checks if health has reached zero to trigger the death sequence.
```
void Update()
{
    health = Mathf.Round(health);
    stamina = Mathf.Round(stamina);

    if (health <= 0)
    {
        ObjectDeath();
    }
}
```
