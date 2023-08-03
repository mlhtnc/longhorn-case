# longhorn case

- I used a StateMachine to manage core game loop. Every task in the game is a state e.g. BoardState, BinState...
Without StateMachine code base could be more complex with a lot of bools to manage the states.

- Implemented an InputManager to handle all pointer down, up detections. Used static events so any script can register to OnPointerDown and OnPointerUp events easily to be notified.
Also implemented two interfaces (IPointerDownHandler, IPointerUpHandler) to notify events when triggered on a specific gameObject who implements either of the interfaces.
This design choice also match with Interface Segregation.

- Implemented two classes (ClickableObject, DraggableObject) to manage clicks and drag events. DraggableObject inherited from ClickableObject. This choice may change in future,
because there maybe a situation like where Draggable Object is not a ClickableObject.