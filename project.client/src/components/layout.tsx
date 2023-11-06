import { Menu } from "./menu";

type LayoutProps = {
    children: React.ReactNode
}

export function Layout(props: LayoutProps) {
    return (
        <div>
            <Menu />
            <div className="pt-5">
                {props.children}
            </div>
        </div>
    );
}