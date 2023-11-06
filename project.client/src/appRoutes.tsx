import { Navigate } from "react-router-dom";
import HomePage from "./components/pages/homePage";
import LoginPage from "./components/pages/loginPage";
import TagsPage from "./components/pages/tagsPage";

class AppRoute {
    public index: boolean;
    public path: string;
    public element: React.ReactNode;
    public isPrivate: boolean;

    constructor(index: boolean, path: string, element: React.ReactNode, isPrivate: boolean) {
        this.index = index;
        this.path = path;
        this.element = element;
        this.isPrivate = isPrivate;
    }
}

const appRoutes: AppRoute[] = [
    {
        index: true,
        path: '/',
        element: <HomePage />,
        isPrivate: true
    },
    {
        index: false,
        path: '/stack-tags',
        element: <TagsPage />,
        isPrivate: true
    },
    {
        index: false,
        path: '/login',
        element: <LoginPage />,
        isPrivate: false
    },
    {
        index: false,
        path: '*',
        element: <Navigate to="/" />,
        isPrivate: false
    }
]

export default appRoutes;