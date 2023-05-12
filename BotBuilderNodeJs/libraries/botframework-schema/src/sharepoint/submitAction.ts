// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { IAction } from './IAction';
import { ConfirmationDialog } from './confirmationDialog';

/**
 * Sharepoint action.submit 
 */
export class SubmitAction implements IAction{
    private type: string = 'Submit';
    private parameters: { [key: string] : unknown };
    private confirmationDialog: ConfirmationDialog;

    /**
     * Initializes a new instance of the SubmitAction class
     */
    public SubmitAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type key:value pair
     */
    public set Parameters (parameters: { [key: string] : unknown }) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type key:value pair
     */
    public get Parameters(): { [key: string] : unknown } {
        return this.parameters; 
    }

    /**
     * Sets confirmation dialog property of type ConfirmationDialog
     */
    public set ConfirmationDialog (confirmationDialog: ConfirmationDialog) {
        this.confirmationDialog = confirmationDialog;
    }

    /**
     * Gets confirmation dialog property of type ConfirmationDialog
     */
    public get ConfirmationDialog(): ConfirmationDialog {
        return this.confirmationDialog; 
    }
}