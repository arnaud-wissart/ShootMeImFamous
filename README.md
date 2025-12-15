# ShootMeImFamous – Checklist Unity "Space Rocks" (primitives seulement)

Checklist rapide pour configurer l'éditeur Unity afin de faire tourner le MVP "Space Rocks" en n'utilisant que des primitives. Les scripts disponibles sont `ShipController`, `Projectile`, `Asteroid`, `AsteroidSpawner`, `CameraFollow` et `GameManager`.

## Préparation de la scène
- [ ] Créer une nouvelle scène **Main** et l'enregistrer immédiatement.

## Player (vaisseau)
- [ ] Ajouter un GameObject **Player** avec une primitive **Capsule** (ou **Cylinder**).
- [ ] Ajouter un **Rigidbody** (désactiver *Use Gravity*, activer *Continuous* en *Collision Detection*, optionnellement *Interpolate = Interpolate*).
- [ ] Vérifier le **Collider** généré par la primitive (CapsuleCollider ou MeshCollider selon le cas).
- [ ] Créer un enfant **Muzzle** (Empty) positionné en avant du vaisseau (sur l'axe +Z ou +Y selon l'orientation du modèle).
- [ ] Attacher le script **ShipController** sur Player et renseigner :
  - **Muzzle** : la référence de l'enfant créé.
  - **Projectile Prefab** : le prefab Projectile décrit ci-dessous.

## Projectile (prefab)
- [ ] Créer une primitive **Sphere** de petite taille, la nommer **Projectile**.
- [ ] Ajouter un **Rigidbody** (désactiver *Use Gravity*, mettre *Collision Detection = Continuous*, *Interpolate = Interpolate* recommandé).
- [ ] Conserver le **SphereCollider** (ou ajuster le rayon si besoin).
- [ ] Attacher le script **Projectile**.
- [ ] Glisser l'objet dans le dossier Prefabs pour en faire un **prefab** et utiliser ce prefab dans **ShipController.ProjectilePrefab**.

## Asteroids (prefabs)
- [ ] Créer une primitive **Sphere**, la nommer **Asteroid**.
- [ ] Ajouter un **Rigidbody** (désactiver *Use Gravity*, *Collision Detection = Continuous Speculative* ou *Continuous* selon la vitesse, *Interpolate = Interpolate* conseillé).
- [ ] Conserver un **SphereCollider** (ajuster le rayon ou le scale selon la taille désirée).
- [ ] Attacher le script **Asteroid** et définir le champ **Size** (Large/Medium/Small ou une valeur numérique).
- [ ] Sauvegarder **3 prefabs** (Large, Medium, Small) ou un **prefab unique paramétrable**. Si le script exige un auto-référencement pour le split, renseigner **AsteroidPrefab = ce prefab**.

## Gestion du jeu
- [ ] Ajouter un GameObject **GameManager** à la racine et y attacher le script **GameManager** (renseigner les références requises s'il y en a : joueur, UI, spawner, etc.).

## Caméra
- [ ] Sélectionner **Main Camera** et passer en **Orthographic = true**.
- [ ] Régler **Size** (ex. 8–12 selon l'arène ; ajuster pour voir l'aire de jeu complète).
- [ ] Attacher le script **CameraFollow** et affecter **Target = Player**.

## Spawner d'astéroïdes
- [ ] Créer un GameObject **AsteroidSpawner**.
- [ ] Attacher le script **AsteroidSpawner** et renseigner :
  - **Player** : référence du Player pour éviter le spawn trop proche.
  - **Asteroid Prefab** : prefab Large (ou liste de prefabs si supportée).
  - **Paramètres** : rayon de spawn, cadence/délai initial, vitesse minimale/maximale.

## Paramètres Physics et collisions
- [ ] Vérifier que **Use Gravity** est désactivé sur **Player**, **Projectile** et **Asteroid**.
- [ ] Si souhaité, créer des **Layers** (ex. `Player`, `Projectile`, `Asteroid`) et ajuster la **Collision Matrix** pour autoriser : Player↔Asteroid, Projectile↔Asteroid, éviter Projectile↔Player si nécessaire.
- [ ] Optionnel : fixer **Default Material** (Physic Material) à friction nulle pour des mouvements plus arcades.

## Test rapide du MVP
- [ ] Entrer en **Play Mode**.
- [ ] Vérifier les déplacements du vaisseau et la rotation.
- [ ] Tirer : le projectile part du **Muzzle** et détruit/sectionne les astéroïdes.
- [ ] Valider le **split** des astéroïdes (Large → Medium → Small) et leur mouvement.
- [ ] Confirmer que la **collision astéroïde/vaisseau** est létale et déclenche l'état de fin.
- [ ] Appuyer sur **R** pour relancer la partie via **GameManager**.
