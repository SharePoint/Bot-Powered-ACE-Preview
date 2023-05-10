// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint confirmation dialog for SubmitAction
 */
export class ConfirmationDialog {
    private title: string;
    private message: string;

    /**
     * Initializes a new instance of the ConfirmationDialog class
     */
    public ConfirmationDialog() {
        // Do nothing
    }

    /**
     * Sets title property of type string
     */
    public set Title(title: string) {
        this.title = title;
    }

    /**
     * Gets title property of type string
     */
    public get Title(): string {
        return this.title;
    }

    /**
     * Sets message property of type string
     */
     public set Message(message: string) {
        this.message = message;
    }

    /**
     * Gets message property of type string
     */
    public get Message(): string {
        return this.message;
    }
}