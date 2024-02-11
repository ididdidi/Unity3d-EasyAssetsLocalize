# Unity3d-EasyAssetsLocalize

![Pic. 1. View](https://ididdidi.ru/assets/images/projects/easy-assets-localize/view.jpg?raw=true "Pic. 1. View") 
The package for easy localization of assets in Unity3d.

## 1. Installation
* Open the __Package Manager__: `Window` -> `Package Manager`.
* Click on the __`+`__ at the top.
* Select `Add package from git URL...` in the drop-down list.
* Paste the [link to this repository](https://github.com/ididdidi/Unity3d-EasyAssetsLocalize.git) into the field that appears and press `Enter`.
* Wait for the code generation to finish.
> [!Warning]
> Do not change the package folder structure, otherwise you risk losing all localization data.

## 2. Setup
Select from the top menu: `Window` -> `Localization Storage`.

![Pic. 2. Settings button](https://ididdidi.ru/assets/images/projects/easy-assets-localize/settings-button.jpg?raw=true "Pic. 2. Settings button")
<center>In the Localization Storage window, click on the ⚙ button near the search field.</center>

![Pic. 3. Settings](https://ididdidi.ru/assets/images/projects/easy-assets-localize/settings.jpg?raw=true "Pic. 3. Settings")
<center>In the tab, you can add the languages and resource types needed for localization.</center>

![Pic. 4. Add language](https://ididdidi.ru/assets/images/projects/easy-assets-localize/languages.jpg?raw=true "Pic. 4. Add language")
<center>Click on the <b>+</b> at the bottom of the list and select it from the list of Unity's base system languages to add a language.</center>

![Pic. 5. Add resource type](https://ididdidi.ru/assets/images/projects/easy-assets-localize/types.jpg?raw=true "Pic. 5. Add resource type")
<center>Click the <b>+</b> at the end of the list with types, to add a new type of assets for localization.</center>

![Pic. 6. Settings](https://ididdidi.ru/assets/images/projects/easy-assets-localize/settings-result.jpg?raw=true "Pic. 6. Settings")
<center>Drag the default asset sample into the field that appears. Wait for the code generation to finish.</center>

![Pic. 7. New resource type](https://ididdidi.ru/assets/images/projects/easy-assets-localize/new-resource-type.jpg?raw=true "Pic. 7. New resource type") 
<center>A new item of the corresponding type will appear in the list of localizations. When you click on it, a list of localizations of this type will open.</center>

![Pic. 8. Default localization](https://ididdidi.ru/assets/images/projects/easy-assets-localize/default-localization.jpg?raw=true "Pic. 8. Default localization") 
<center>The list of localizations of this type will contain the default localization.</center>

You can edit the default localization by selecting it and clicking on the tools icon in the upper right corner of the localization window.

![Pic. 9. Edit default localization](https://ididdidi.ru/assets/images/projects/easy-assets-localize/edit-default-localization.jpg?raw=true "Pic. 9. Edit default localization")
<center>This unlocks the resources of the languages used to change.</center>

## 3. Applying localization to an object
Select the object that needs localization.
Add a resource localization component to it: `Add component` -> `Localize` -> `[Resource type]Localization`.
![Pic. 10. Add Localization Component](https://ididdidi.ru/assets/images/projects/easy-assets-localize/localization-component.png?raw=true "Pic. 7. Add Localization Component")

![Pic. 11. Localization Component](https://ididdidi.ru/assets/images/projects/easy-assets-localize/localization-component-view.jpg?raw=true "Pic. 8. Localization Component")<center>Add the object to the list of handlers and select the property corresponding to the resource type (usually they are at the top of the drop-down list)</center>

![Pic. 12. Add handler](https://ididdidi.ru/assets/images/projects/easy-assets-localize/add-handler.jpg?raw=true "Pic. 9. Add handler")
<center>The new localization will link to the same resources as the standard one.</center>

## 4. Change localization

You can change the localization directly on the object. To do this, click the `Change Localization` button in the inspector window of the localization component.

![Pic. 13. Change localization](https://ididdidi.ru/assets/images/projects/easy-assets-localize/change-localization.jpg?raw=true "Pic. 10. Change localization")
<center>In the list that appears, you can select an existing localization or add a new one.</center>

The new localization will link to the same resources as the standard one. They can be changed directly on the component.

> Attention! When editing a localization used by several objects, the changes will affect all objects using an instance of this localization.

## 5. Managing localizations in Runtime

To control localization in Runtime, you need to add the `LocalizationController` component to the stage.

![Pic. 14. LocalizationController](https://ididdidi.ru/assets/images/projects/easy-assets-localize/localization-controller.jpg?raw=true "Pic. 14. LocalizationController")

**Scriptable Object** of type `LocalizationStorage` must be added to the `Localization Storage` field.
In the `On Chage Language` event field you can add a handler that receives a string with the name of the language.

To interact with an object of type `LocalizationController` in code, you can use the following methods:

Method          | Description
----------------|---------
GetInstance     | A static method that creates or returns a ready-made instance from the stage. Accepts a `dontDestroy` argument which allows you to save the object instance when changing scenes. Returns an instance of `LocalizationController`.
Subscribe       | Allows an instance of a `LocalizationComponent` derived type, passed as an argument, to subscribe to localization changes.
Unsubscribe     | Allows an instance of a `LocalizationComponent` derived type, passed as an argument, to subscribe to localization changes.
SetNextLanguage | Changes the language to the next one in the localization list.
SetPrevLanguage | Changes the language to the previous one in the localization list.
SetLanguage     | Sets the current language and loads localized resources. Takes an object of type Language as an argument
