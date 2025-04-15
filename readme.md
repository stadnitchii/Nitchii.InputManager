### Description
A small library for helping simulate and react to mouse and keyboard input
It only supports windows using user32.dll


### KeyboardInput
```cs
// Simple key presss
KeyboardInput.KeyDown(Keys.G);
await Task.Delay(50);
KeyboardInput.KeyUp(Keys.G);

// Alternatively use the key press, default of 50ms delay between key up and key down
await KeyboardInput.KeyPress(Keys.G);

// or specify delay
await KeyboardInput.KeyPress(Keys.G, 500);
```

### MouseInput
```cs
// Simple key presss
MouseInput.ButtonDown(MouseButtons.Left);
await Task.Delay(50);
MouseInput.ButtonUp(MouseButtons.Left);

// Alternatively use the key press, default of 50ms delay between key up and key down
await MouseInput.ButtonPress(MouseButtons.Left);

// or specify delay
await MouseInput.ButtonPress(MouseButtons.Left, 5000);
```