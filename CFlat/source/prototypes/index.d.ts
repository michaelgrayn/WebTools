interface String {
    /**
     * Checks if the string is undefined, null, empty, or whitespace.
     */
    isBlank(): boolean;
}

interface Array<T> {
    /**
     * Removes elements form the array, and returns the new length of the array.
     * @param items The elements to remove.
     */
    pop(...items: T[]): void;

    /**
     * Removes the element at the given index and returns it.
     * @param index The index of the element to remove.
     */
    popAt(index: number): T;

    /**
     * Gets the first element of the array or undefined if empty.
     */
    first(): T;

    /**
     * Gets the last element of the array or undefined if empty.
     */
    last(): T;
}
