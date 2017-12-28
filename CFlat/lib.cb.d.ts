declare interface String {
    /**
     * Checks if the string is undefined, null, empty, or whitespace.
     */
    isBlank(): boolean;
}

declare interface Array<T> {
    /**
     * Adds an element to the end of the array.
     * @param element The element to add.
     */
    add(element: T): void;

    /**
     * Removes the element at the end of the array.
     */
    remove(): void;

    /**
     * Removes an element form the array.
     * @param element The element to remove.
     */
    remove(element: T): void;

    /**
     * Removes the element at the given index.
     * @param index The index of the element to remove
     */
    removeAt(index: number): void;
}
