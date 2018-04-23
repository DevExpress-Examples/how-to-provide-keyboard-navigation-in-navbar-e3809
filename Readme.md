# How to provide keyboard navigation in NavBar


<p>This example demonstrates how to allow an end-user to navigate between NavBar groups and items using the keyboard.</p>
<br />
<p>The solution provides the following key combinations:</p>
<p>- Tab/Shift+Tab to change a group;</p>
<p>- Up/Down to go to the previous/next NavBar item;</p>
<p>- Left/Right to reduce/expand a selected group;</p>
<p> </p>
<br />
<p>To enable this feature in your application, we have created a NavBarKeyboardHelper class, which can be used as attached behavior for NavBarControl. For instance:</p>


```xml
<dxn:NavBarControl>
	<i:Interaction.Behaviors>
		<dxm:NavBarKeyboardHelper/>
	</i:Interaction.Behaviors>
<dxn:NavBarControl>
```


<p> </p>
<p>The Silverlight version has one limitation: it does not support themes. We have adapted only the Seven theme to enable this feature.</p>

<br/>


